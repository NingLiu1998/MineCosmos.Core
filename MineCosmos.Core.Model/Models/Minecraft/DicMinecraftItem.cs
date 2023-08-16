using SqlSugar;

namespace MineCosmos.Core.Model.Models
{
    /// <summary>
    /// Minecraft物品字典表
    /// </summary>
    public class DicMinecraftItem : RootEntityTkey<int>
    {
        /// <summary>
        /// 命名空间ID,和wiki一样的原类型名（原版/模组）
        /// </summary>
        [SugarColumn(Length = 255, IsNullable = true)]
        public string NamespacedIdentifier { get; set; }

        /// <summary>
        ///  JE/BE 不考虑其他的
        /// </summary>
        [SugarColumn(Length = 10, IsNullable = true)]
        public string Platform { get; set; }

        /// <summary>
        ///  关联材质包字典表，如果是原版则为0
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int MaterialId { get; set; }

        /// <summary>
        ///  关联模组字典表，如果是原版则为0
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int ModuleId { get; set; }

        /// <summary>
        ///  别名 （中文名）
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = true)]
        public string Alias { get; set; }
    }
}
