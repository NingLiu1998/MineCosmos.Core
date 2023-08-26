using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MineCosmos.Core.IServices.BASE;
using MineCosmos.Core.Model.Models;
using MineCosmos.Core.Model.ViewModels.Minecraft;

namespace MineCosmos.Core.IServices.Minecraft
{
    public interface IWareHouseService : IBaseServices<PlayerWareHouse>
    {
        /// <summary>
        /// 自动新建玩家默认仓库，如果玩家默认仓库已经存在则返回玩家仓库
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="wareHouseName"></param>
        /// <returns></returns>
        Task<PlayerWareHouse> AutoCreateDefaultWareHouseAsync(PlayerWareHouseCreateDto model);
        Task<List<PlayerWareHouseCreateDto>> GetPlayerAllWareHouse(int playerId);
        Task<string> GetWareHouseItemByPlayer(string uuid, int slot);
    }
}
