using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        //[Required(ErrorMessage ="缺少传递物品数据信息")]
        public string Source { get; set; }
        [Required(ErrorMessage = "缺少玩家信息")]
        public string UUID { get; set; }
        [Required(ErrorMessage = "缺少玩家信息")]
        public string Name { get; set; }
        [Required(ErrorMessage = "缺少服务器版本信息")]
        public string SourceVersion { get; set; }
        public string ApiKey { get; set; } = string.Empty;
        //public int Solt { get; set; } = 1;
    }
}
