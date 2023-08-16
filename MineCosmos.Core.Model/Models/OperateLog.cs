using SqlSugar;
using System;

namespace MineCosmos.Core.Model.Models
{
    /// <summary>
    /// 日志记录
    /// </summary>
    public class OperateLog : RootEntityTkey<int>
    {
        /// <summary>
        /// 区域名
        /// </summary>
        [SugarColumn(Length = 2000, IsNullable = true)]
        public string Area { get; set; }
        /// <summary>
        /// 区域控制器名
        /// </summary>
        [SugarColumn(Length = 2000, IsNullable = true)]
        public string Controller { get; set; }
        /// <summary>
        /// Action名称
        /// </summary>
        [SugarColumn(Length = 2000, IsNullable = true)]
        public string Action { get; set; }
        /// <summary>
        /// IP地址
        /// </summary>
        [SugarColumn(Length = 2000, IsNullable = true)]
        public string IPAddress { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [SugarColumn(Length = 2000, IsNullable = true)]
        public string Description { get; set; }
        /// <summary>
        /// 登录时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? LogTime { get; set; }
        /// <summary>
        /// 登录名称
        /// </summary>
        [SugarColumn(Length = 2000, IsNullable = true)]
        public string LoginName { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }

        [SugarColumn(IsIgnore = true)]
        public virtual SysUserInfo User { get; set; }
    }
}
