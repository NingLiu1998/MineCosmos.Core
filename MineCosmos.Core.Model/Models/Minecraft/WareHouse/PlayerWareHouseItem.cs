using SqlSugar;

namespace MineCosmos.Core.Model.Models;

/// <summary>
/// 玩家仓库子表
/// </summary>
public class PlayerWareHouseItem : RootEntityTkey<int>
{
    /// <summary>
    /// 主表仓库ID
    /// </summary>
    [SugarColumn(IsNullable = false)]
    public int WareHouseId { get; set; }

    /// <summary>
    /// Minecraft物品数据 （就是Minecraft里面一个ItemStack的数据）
    /// </summary>
    [SugarColumn(IsNullable = true,ColumnDataType ="text")]
    public string ItemData { get; set; }

    /// <summary>
    /// 物品类型（可能产生的json数据会不一样，所以这里区分)
    /// </summary>
    public WareHouseItemTypeEnum ItemType { get; set; }

    /// <summary>
    /// 所在栏位 (主要作用于前端界面展示
    /// </summary>
    public int Slot { get; set; }

}

public enum WareHouseItemTypeEnum
{
    插件 = 100,
    模组 = 200,
}

