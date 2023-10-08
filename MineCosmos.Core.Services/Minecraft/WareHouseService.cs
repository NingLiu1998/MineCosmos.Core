using Mapster;
using MineCosmos.Core.Common;
using MineCosmos.Core.Common.Helper;
using MineCosmos.Core.Common.Static;
using MineCosmos.Core.IRepository.Base;
using MineCosmos.Core.IServices;
using MineCosmos.Core.IServices.Minecraft;
using MineCosmos.Core.Model.Models;
using MineCosmos.Core.Model.ViewModels.Minecraft;
using MineCosmos.Core.Services.BASE;
using Newtonsoft.Json.Linq;
using SharpNBT;
using SharpNBT.SNBT;
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
        readonly IBaseRepository<MinecraftPlayer> _playerService;
        public WareHouseServiceService(IBaseRepository<PlayerWareHouseItem> mcPlayerWareHouseItem,
            IBaseRepository<MinecraftPlayer> playerService)
        {
            _mcPlayerWareHouseItem = mcPlayerWareHouseItem;
            _playerService = playerService;
        }

        #region 仓库功能


        /// <summary>
        /// 仓库整理
        /// *目的就是把相同类型不满一组的物品合并到一个格子
        /// </summary>
        /// <param name="wareHouseId">仓库ID</param>
        /// <returns></returns>
        public async Task TidyUp(int wareHouseId)
        {
            var has = await AnyAsync(a => a.Id == wareHouseId);
            if (!has) return;
            var lstWareHouseInfo = await _mcPlayerWareHouseItem.GetListAsync
                 (a => a.WareHouseId == wareHouseId);
            if (lstWareHouseInfo.Count <= 0) return;

            //先拿到转换后的Tag集合
            List<CompoundTag> lstTag = new();
            foreach (var item in lstWareHouseInfo) lstTag.Add(StringNbt.Parse(item.ItemData));



            //minecraft:iron_ingot

            //lstTag.Where(a=>a.item)

            foreach (CompoundTag tagItem in lstTag)
            {
                tagItem.TryGetValue(NbtHelper.NBTID, out StringTag id);
                switch (id)
                {
                    case "minecraft:iron_ingot":

                        break;

                    default:
                        break;
                }

            }





        }

        #endregion

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

                int slot = hasItemCount;
                bool hasSlot = true;
                while (hasSlot)
                {
                    slot++;
                    hasSlot = await _mcPlayerWareHouseItem
                   .AnyAsync(a => a.Slot == slot && a.WareHouseId == playerWareHouse.Id);

                }

                model.Items.ForEach(a =>
                {
                    a.WareHouseId = playerWareHouse.Id;
                    a.Slot = slot;
                });
                //  model.Items.Adap

                var items = model.Items.Adapt<List<PlayerWareHouseItem>>();

                //批量添加物品
                await _mcPlayerWareHouseItem.Add(items);
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
            var lstWareHouse = await GetListAsync(a => a.PlayerId == playerId && !a.IsDeleted.Value);

            if (lstWareHouse.Count <= 0) throw Oops.Bah("玩家没有仓库");

            var lstId = lstWareHouse.Select(a => a.Id).ToList();
            List<PlayerWareHouseItem> lstWareHouseItem = await _mcPlayerWareHouseItem.GetListAsync(a => lstId.Contains(a.WareHouseId) && !a.IsDeleted.Value);

            List<PlayerWareHouseCreateDto> lst = new();

            #region 物品NBT 转 JSON  TODO:封装
            var lstItem = new List<PlayerWareHouseItemDto>();
            foreach (PlayerWareHouseItem wareHouseItem in lstWareHouseItem)
            {
                //暂时这样处理无法转换的物品
                try
                {
                    CompoundTag tags = StringNbt.Parse(wareHouseItem.ItemData);

                    //var nbtDic = NbtHelper.NbtStrToDic(wareHouseItem.ItemData);
                    lstItem.Add(new PlayerWareHouseItemDto
                    {
                        ItemType = wareHouseItem.ItemType,
                        NbtJson = (JObject)(JArray.Parse(tags.ToJson())).FirstOrDefault(),
                        Slot = wareHouseItem.Slot,
                        WareHouseId = wareHouseItem.WareHouseId,
                        NbtId = wareHouseItem.Id
                    });
                }
                catch (System.Exception)
                {
                    continue;
                }

            }
            #endregion

            foreach (PlayerWareHouse wareHouse in lstWareHouse)
            {
                lst.Add(new()
                {
                    Items = lstItem,
                    WareHouse = wareHouse
                });
            }

            return lst;
        }


        /// <summary>
        /// 获取玩家默认仓库指定槽位物品的原始nbt字符串
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="slot"></param>
        /// <returns></returns>
        public async Task<string> GetWareHouseItemByPlayer(string uuid, int slot)
        {
            var playerInfo = await _playerService.GetAsync(a => a.UUID == uuid);

            if (playerInfo is null) throw Oops.Bah($"玩家信息不存在");

            var defaultWareHouse = await GetAsync(a => a.PlayerId == playerInfo.Id && a.Type == PlayerWareHouseTypeEnum.默认仓库);

            if (defaultWareHouse is null) throw Oops.Bah($"玩家默认仓库不存在");

            var item = await _mcPlayerWareHouseItem.GetAsync(a => a.WareHouseId == defaultWareHouse.Id && a.Slot == slot);

            if (item.ItemData.IsNullOrEmpty()) throw Oops.Bah($"默认仓库{defaultWareHouse.Name}的{slot}槽位物品数据异常");

            return item.ItemData;

        }
    }
}
