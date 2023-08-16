using SqlSugar;
using System;

namespace MineCosmos.Core.Model.Models
{
    /// <summary>
    /// 接口API地址信息表
    /// </summary>
    public class Modules : ModulesRoot<int>
    {
        public Modules()
        {
            //this.ChildModule = new List<Module>();
            //this.ModulePermission = new List<ModulePermission>();
            //this.RoleModulePermission = new List<RoleModulePermission>();
        }

        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public string Name { get; set; }
        /// <summary>
        /// 菜单链接地址
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = true)]
        public string LinkUrl { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        [SugarColumn(Length = 2000, IsNullable = true)]
        public string Area { get; set; }
        /// <summary>
        /// 控制器名称
        /// </summary>
        [SugarColumn(Length = 2000, IsNullable = true)]
        public string Controller { get; set; }
        /// <summary>
        /// Action名称
        /// </summary>
        [SugarColumn(Length = 2000, IsNullable = true)]
        public string Action { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = true)]
        public string Icon { get; set; }
        /// <summary>
        /// 菜单编号
        /// </summary>
        [SugarColumn(Length = 10, IsNullable = true)]
        public string Code { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int OrderSort { get; set; }
        /// <summary>
        /// /描述
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = true)]
        public string Description { get; set; }
        /// <summary>
        /// 是否是右侧菜单
        /// </summary>
        public bool IsMenu { get; set; }
        /// <summary>
        /// 是否激活
        /// </summary>
        public bool Enabled { get; set; }

        //public virtual Module ParentModule { get; set; }
        //public virtual ICollection<Module> ChildModule { get; set; }
        //public virtual ICollection<ModulePermission> ModulePermission { get; set; }
        //public virtual ICollection<RoleModulePermission> RoleModulePermission { get; set; }
    }
}
