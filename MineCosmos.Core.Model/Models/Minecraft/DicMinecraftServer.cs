using SqlSugar;

namespace MineCosmos.Core.Model.Models
{
    public class DicMinecraftServer
    {
        /// <summary>
        /// 服务器名称
        /// </summary>
        [SugarColumn(Length = 100)]
        public string ServerName { get; set; }

        /// <summary>
        /// 当前周目
        /// </summary>
        [SugarColumn(Length = 1)]
        public int CurrentPtc { get; set; }

        /// <summary>
        /// 连接地址,单个: “192.168.1.1:123456”，多个用;分割
        /// </summary>
        [SugarColumn(Length = 100)]
        public string ConnectAddress { get; set; }
    }
}
