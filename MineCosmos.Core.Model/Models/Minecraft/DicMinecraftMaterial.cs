using SqlSugar;

namespace MineCosmos.Core.Model.Models;
public class DicMinecraftMaterial : RootEntityTkey<int>
{
    /// <summary>
    /// 材质包名称
    /// </summary>
    [SugarColumn(Length = 50, IsNullable = true)]
    public string Name { get; set; }

    /// <summary>
    /// CDN地址
    /// </summary>
    [SugarColumn(IsNullable = true)]
    public string CDNUrl { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    [SugarColumn(IsNullable = true)]
    public bool Enable { get; set; }
}

