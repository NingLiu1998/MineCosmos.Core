using SqlSugar;
using System;
using System.Collections.Generic;

namespace MineCosmos.Core.Model.Models
{
    /// <summary>
    /// 路由菜单表
    /// </summary>
    public class Permission : PermissionRoot<int>
    {
        public Permission()
        {
            //this.ModulePermission = new List<ModulePermission>();
            //this.RoleModulePermission = new List<RoleModulePermission>();
        }

        /// <summary>
        /// 菜单执行Action名
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public string Code { get; set; }
        /// <summary>
        /// 菜单显示名（如用户页、编辑(按钮)、删除(按钮)）
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public string Name { get; set; }
        /// <summary>
        /// 是否是按钮
        /// </summary>
        public bool IsButton { get; set; } = false;
        /// <summary>
        /// 是否是隐藏菜单
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public bool? IsHide { get; set; } = false;
        /// <summary>
        /// 是否keepAlive
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public bool? IskeepAlive { get; set; } = false;


        /// <summary>
        /// 按钮事件
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = true)]
        public string Func { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int OrderSort { get; set; }
        /// <summary>
        /// 菜单图标
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = true)]
        public string Icon { get; set; }
        /// <summary>
        /// 菜单描述    
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = true)]
        public string Description { get; set; }
        /// <summary>
        /// 激活状态
        /// </summary>
        public bool Enabled { get; set; }     

        [SugarColumn(IsIgnore = true)]
        public List<string> PnameArr { get; set; } = new List<string>();
        [SugarColumn(IsIgnore = true)]
        public List<string> PCodeArr { get; set; } = new List<string>();
        [SugarColumn(IsIgnore = true)]
        public string MName { get; set; }

        [SugarColumn(IsIgnore = true)]
        public bool hasChildren { get; set; } = true;

        [SugarColumn(IsIgnore = true)]
        public List<Permission> Children { get; set; } = new List<Permission>();

        [SugarColumn(IsIgnore = true)]
        public Modules Module { get; set; }

        //public virtual ICollection<ModulePermission> ModulePermission { get; set; }
        //public virtual ICollection<RoleModulePermission> RoleModulePermission { get; set; }
    }
}
