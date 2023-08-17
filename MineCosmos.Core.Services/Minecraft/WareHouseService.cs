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
    /// Minecraft���
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
            if (playerWareHouse.Count <= 0) throw Oops.Bah("��Ҳֿⲻ����");

            return playerWareHouse;
        }

        /// <summary>
        /// �Զ��½����Ĭ�ϲֿ⣬������Ĭ�ϲֿ��Ѿ������򷵻���Ҳֿ�
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="wareHouseName"></param>
        /// <returns></returns>
        public async Task<PlayerWareHouse> AutoCreateDefaultWareHouseAsync(PlayerWareHouseCreateDto model)
        {

            if (model.WareHouse is null) throw Oops.Bah("�ֿ���Ϣ����Ϊ��");

            if (model.WareHouse.PlayerId <= 0) throw Oops.Bah("�Զ��½��ֿ����ID����Ϊ��");

            PlayerWareHouse playerWareHouse = await GetAsync(a =>
            a.PlayerId == model.WareHouse.PlayerId
            && !a.IsDeleted.Value
            && a.Type == PlayerWareHouseTypeEnum.Ĭ�ϲֿ�);

            int hasItemCount = 0;
            if (playerWareHouse is null)
            {
                playerWareHouse = await InsertReturnEntity(model.WareHouse);
                hasItemCount = await _mcPlayerWareHouseItem.CountAsync(a => a.WareHouseId == playerWareHouse.Id);
            }
            else
            {
                playerWareHouse = model.WareHouse;
                await Add(playerWareHouse);
            }


            if (model.Items.Count > 0)
            {
                if (model.Items.Count + hasItemCount > playerWareHouse.UpperLimit)
                    throw Oops.Bah("�����Ʒ���ֿ�ʧ�ܣ��ѳ����ֿ�洢����");

                model.Items.ForEach(a => a.WareHouseId = playerWareHouse.Id);

                //���������Ʒ
                await _mcPlayerWareHouseItem.Add(model.Items);
            }


            return playerWareHouse;
        }


        /// <summary>
        /// ��ȡ������вֿ���Ϣ
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public async Task<List<PlayerWareHouseCreateDto>> GetPlayerAllWareHouse(int playerId)
        {

           var lstWareHouse = await  GetListAsync(a=>a.PlayerId == playerId && !a.IsDeleted.Value);

            if(lstWareHouse.Count <=0) throw Oops.Bah("���û�вֿ�");

            var lstId = lstWareHouse.Select(a=>a.Id).ToList();
            var lstWareHouseItem = _mcPlayerWareHouseItem.GetListAsync(a => lstId.Contains(a.WareHouseId) && !a.IsDeleted.Value);

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
