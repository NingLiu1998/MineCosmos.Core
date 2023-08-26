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
        public List<PlayerWareHouseItemDto> Items { get; set; } = null;
    }

    public class PlayerWareHouseItemDto
    {
        public int NbtId { get; set; }

        /// <summary>
        /// 主表仓库ID
        /// </summary>
        public int WareHouseId { get; set; }


        /// <summary>
        /// 原始NBT字符串数据
        /// </summary>
        public string ItemData { get; set; }

        /// <summary>
        /// 物品NBT转换后的json对象
        /// </summary>
        public dynamic NbtJson { get; set; }

        /// <summary>
        /// 物品类型（可能产生的json数据会不一样，所以这里区分)
        /// </summary>
        public WareHouseItemTypeEnum ItemType { get; set; }

        /// <summary>
        /// 所在栏位 (这个代表的是云平台仓库栏位，和mc无关
        /// </summary>
        public int Slot { get; set; }
    }
}
