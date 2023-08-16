using System;
using SqlSugar;

namespace MineCosmos.Core.Model.Models
{
    /// <summary>
    /// 玩家信息
    /// </summary>
    public class MinecraftPlayer : RootEntityTkey<int>
    {
        /// <summary>
        /// 玩家名称（纯英文）
        /// </summary>
        [SugarColumn(Length = 100)]
        public virtual string Name { get; set; }

        /// <summary>
        /// 正版唯一ID
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public virtual string UUID { get; set; }

        /// <summary>
        /// KookID
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public virtual string KookUserId { get; set; }

        /// <summary>
        /// DiscordId
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public virtual string DiscordId { get; set; }

        /// <summary>
        /// qq
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public virtual string QQ { get; set; }

        /// <summary>
        /// 加入方式：如：原版服务器、模组服务器、Kook
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string JoinType { get; set; }

        /// <summary>
        /// 最后网页登录时间
        /// </summary>
        public DateTime? LastWebLoginTime { get; set; }
        /// <summary>
        /// 最后进服时间
        /// </summary>
        public DateTime? LastServerLoginTime { get; set; }
        /// <summary>
        /// Kook最后登录时间
        /// </summary>
        public DateTime? LastLoginKookTime { get; set; }
    }
}
