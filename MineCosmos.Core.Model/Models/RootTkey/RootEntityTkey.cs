using SqlSugar;
using System;

namespace MineCosmos.Core.Model
{
    public class RootEntityTkey<Tkey> where Tkey : IEquatable<Tkey>
    {
        /// <summary>
        /// ID
        /// 泛型主键Tkey
        /// </summary>
        [SugarColumn(IsNullable = false, IsPrimaryKey = true)]
        public Tkey Id { get; set; }

        /// <summary> 
        /// 修改时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime ModifyTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public System.DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建ID
        /// </summary>        
        [SugarColumn(IsNullable = true)]
        public int? CreateId { get; set; }

        /// <summary>
        /// 修改ID
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? ModifyId { get; set; }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public bool? IsDeleted { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 2000, IsNullable = true)]
        public string Remark { get; set; }
    }
}