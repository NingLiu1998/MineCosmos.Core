using MineCosmos.Core.Common;
using MineCosmos.Core.Common.Static;
using MineCosmos.Core.IRepository.Base;
using MineCosmos.Core.IServices;
using MineCosmos.Core.IServices.Minecraft;
using MineCosmos.Core.Model.Models;
using MineCosmos.Core.Model.ViewModels.Minecraft;
using MineCosmos.Core.Services.BASE;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MineCosmos.Core.Services
{
    /// <summary>
    /// Minecraft玩家
    /// </summary>	
    public class WareHouseServiceService : BaseServices<PlayerWareHouse>, IWareHouseService
    {

        readonly IBaseRepository<PlayerWareHouseItem> _mcPlayerWareHouseItem;
        public WareHouseServiceService(IBaseRepository<PlayerWareHouseItem> mcPlayerWareHouseItem)
        {
            _mcPlayerWareHouseItem = mcPlayerWareHouseItem;
        }

        public async Task<List<PlayerWareHouse>> GetPlayerWareHouseByPlayerId(int playerId)
        {
            var playerWareHouse = await base.GetListAsync(a => a.PlayerId == playerId);
            if (playerWareHouse.Count <= 0) throw Oops.Bah("玩家仓库不存在");

            return playerWareHouse;
        }

        /// <summary>
        /// 自动新建玩家默认仓库，如果玩家默认仓库已经存在则返回玩家仓库
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="wareHouseName"></param>
        /// <returns></returns>
        public async Task<PlayerWareHouse> AutoCreateDefaultWareHouseAsync(PlayerWareHouseCreateDto model)
        {

            if (model.WareHouse is null) throw Oops.Bah("仓库信息不可为空");

            if (model.WareHouse.PlayerId <= 0) throw Oops.Bah("自动新建仓库玩家ID不可为空");

            PlayerWareHouse playerWareHouse = await GetAsync(a =>
            a.PlayerId == model.WareHouse.PlayerId
            && !a.IsDeleted.Value
            && a.Type == model.WareHouse.Type);

            int hasItemCount = 0;
            if (playerWareHouse is null)
            {
                playerWareHouse = await InsertReturnEntity(model.WareHouse);
            }
            else
            {
                hasItemCount = await _mcPlayerWareHouseItem.CountAsync(a => a.WareHouseId == playerWareHouse.Id);
            }

         

            if (model.Items.Count > 0)
            {
                if (model.Items.Count + hasItemCount > playerWareHouse.UpperLimit)
                    throw Oops.Bah("添加物品到仓库失败，已超出仓库存储上限");

                model.Items.ForEach(a => a.WareHouseId = playerWareHouse.Id);

                //批量添加物品
                await _mcPlayerWareHouseItem.Add(model.Items);
            }

            return playerWareHouse;
        }


        /// <summary>
        /// 获取玩家所有仓库信息
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public async Task<List<PlayerWareHouseCreateDto>> GetPlayerAllWareHouse(int playerId)
        {

           var lstWareHouse = await  GetListAsync(a=>a.PlayerId == playerId && !a.IsDeleted.Value);

            if(lstWareHouse.Count <=0) throw Oops.Bah("玩家没有仓库");

            var lstId = lstWareHouse.Select(a=>a.Id).ToList();
            var lstWareHouseItem =await  _mcPlayerWareHouseItem.GetListAsync(a => lstId.Contains(a.WareHouseId) && !a.IsDeleted.Value);

            List<PlayerWareHouseCreateDto> lst = new();

            foreach (PlayerWareHouse wareHouse in lstWareHouse)
            {
                lst.Add(new()
                {
                     Items = lstWareHouseItem.Where(a=>a.WareHouseId == wareHouse.Id).ToList(),
                     WareHouse = wareHouse
                });
            }

            return lst;
        }
    }
}
