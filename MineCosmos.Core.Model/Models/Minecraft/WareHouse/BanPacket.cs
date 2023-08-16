using SqlSugar;

namespace MineCosmos.Core.Model.Models;

/// <summary>
/// 封包数据
/// </summary>
public class BanPacket : RootEntityTkey<int>
{
    /// <summary>
    /// 用于获取的key，主要通过这个查找
    /// </summary>
    [SugarColumn(IsNullable = false)]
    public string Key { get; set; }

    /// <summary>
    /// 包的md5值
    /// </summary>
    [SugarColumn(IsNullable = false)]
    public int Md5 { get; set; }

    /// <summary>
    /// 包类型
    /// </summary>
    [SugarColumn(IsNullable = false)]
    public BanPacketTypeEnum Enum { get; set; }

}

/// <summary>
/// 封包数据类型，未来可扩展多种不同类型的包
/// </summary>
public enum BanPacketTypeEnum
{
    单个包= 1,
    多个包= 2
}

