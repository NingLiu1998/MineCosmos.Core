using SqlSugar;

namespace MineCosmos.Core.Model.Models;

/// <summary>
/// 玩家仓库
/// </summary>
public class PlayerWareHouse : RootEntityTkey<int>
{
    /// <summary>
    /// 玩家ID
    /// </summary>
    [SugarColumn(IsNullable = false)]
    public int PlayerId { get; set; }

    /// <summary>
    ///  上限（栏位）
    /// </summary>
    [SugarColumn(Length = 10, IsNullable = true)]
    public int UpperLimit { get; set; }

    /// <summary>
    /// 封面
    /// </summary>
    [SugarColumn(Length = 500, IsNullable = true)]
    public string Cover { get; set; }

    /// <summary>
    /// 物品展示材质包ID，为空则默认原版
    /// </summary>
    [SugarColumn(IsNullable = true)]
    public int MaterialId { get; set; }

}

