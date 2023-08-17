using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MineCosmos.Core.Model.Models;

namespace MineCosmos.Core.Model.ViewModels.Minecraft
{
    /// <summary>
    /// 新增仓库Dto
    /// </summary>
    public class PlayerWareHouseCreateDto
    {
        /// <summary>
        /// 仓库
        /// </summary>
        public PlayerWareHouse WareHouse { get; set; }

        /// <summary>
        /// 物品
        /// </summary>
        public List<PlayerWareHouseItem> Items { get; set; } = null;
    }
}
