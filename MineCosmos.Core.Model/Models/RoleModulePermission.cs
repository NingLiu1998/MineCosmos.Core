using SqlSugar;
using System;

namespace MineCosmos.Core.Model.Models
{
    /// <summary>
    /// 按钮跟权限关联表
    /// </summary>
    public class RoleModulePermission : RoleModulePermissionRoot<int>
    {
        public RoleModulePermission()
        {
            //this.Role = new Role();
            //this.Module = new Module();
            //this.Permission = new Permission();

        }

        // 下边三个实体参数，只是做传参作用，所以忽略下
        [SugarColumn(IsIgnore = true)]
        public Role Role { get; set; }
        [SugarColumn(IsIgnore = true)]
        public Modules Module { get; set; }
        [SugarColumn(IsIgnore = true)]
        public Permission Permission { get; set; }
    }
}
