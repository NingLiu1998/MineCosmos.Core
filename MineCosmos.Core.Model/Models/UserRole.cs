using SqlSugar;
using System;

namespace MineCosmos.Core.Model.Models
{
    /// <summary>
    /// 用户跟角色关联表
    /// </summary>
    public class UserRole  : RootEntityTkey<int>
    {
        public UserRole() { }

        public UserRole(int uid, int rid)
        {
            UserId = uid;
            RoleId = rid;
            CreateTime = DateTime.Now;
            IsDeleted = false;
            CreateId = uid;
            CreateTime = DateTime.Now;
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleId { get; set; }
    }
}
