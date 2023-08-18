using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MineCosmos.Core.IServices.BASE;
using MineCosmos.Core.Model.Models;

namespace MineCosmos.Core.IServices.Minecraft
{
    public interface IPlayerService : IBaseServices<MinecraftPlayer>
    {
        /// <summary>
        /// 普通登录
        /// </summary>
        /// <param name="uName">用户名</param>
        /// <param name="uPwd">密码</param>
        /// <returns></returns>
        Task<MinecraftPlayer> LoginAsync(string uName, string uPwd);
    }
}
