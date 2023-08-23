using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineCosmos.Core.Model.ViewModels.Minecraft
{
    /// <summary>
    /// Minecraft 服务器 bukkit传送物品数据入参
    /// </summary>
    public class ReqMinecraftItemDto
    {
        public string Source { get; set; }
        public string UUID { get; set; }
        public string Name { get; set; }
        public string SourceVersion { get; set; }
        public string ApiKey { get; set; }
    }
}
