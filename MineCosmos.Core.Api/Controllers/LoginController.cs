using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MineCosmos.Core.AuthHelper;
using MineCosmos.Core.AuthHelper.OverWrite;
using MineCosmos.Core.Common.Helper;
using MineCosmos.Core.IServices;
using MineCosmos.Core.Model;
using MineCosmos.Core.Model.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MineCosmos.Core.IServices.Minecraft;
using MineCosmos.Core.Model.Models;

namespace MineCosmos.Core.Controllers
{
    /// <summary>
    /// 登录管理【无权限】
    /// </summary>
    [Produces("application/json")]
    [Route("api/Login")]
    [AllowAnonymous]
    public class LoginController : BaseApiController
    {
        readonly ISysUserInfoServices _sysUserInfoServices;
        readonly PermissionRequirement _requirement;
        readonly IPlayerService _playerServices;
        private readonly IRoleModulePermissionServices _roleModulePermissionServices;


        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="sysUserInfoServices"></param>
        /// <param name="playerService"></param>
        /// <param name="requirement"></param>
        /// <param name="roleModulePermissionServices"></param>
        public LoginController(ISysUserInfoServices sysUserInfoServices, 
            PermissionRequirement requirement,
            IRoleModulePermissionServices roleModulePermissionServices,
            IPlayerService playerService)
        {
            _sysUserInfoServices = sysUserInfoServices;
            _requirement = requirement;
            _roleModulePermissionServices = roleModulePermissionServices;
            _playerServices = playerService;
        }


        /// <summary>
        /// 平台用户登录
        /// （相对Minecraft就是OP级别的人员登录，和玩家不一样，单独的系统用户）
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("SysUserLogin")]
        public async Task<MessageModel<TokenInfoViewModel>> SysUserLogin(string name = "", string pass = "")
        {
            string jwtStr = string.Empty;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pass))
                return Failed<TokenInfoViewModel>("用户名或密码不能为空");

            pass = MD5Helper.MD5Encrypt32(pass);

            var user = await _sysUserInfoServices.GetListAsync(d => d.LoginName == name && d.LoginPWD == pass && d.IsDeleted == false);
            if (user.Count > 0)
            {
                var userRoles = await _sysUserInfoServices.GetUserRoleNameStr(name, pass);
                //如果是基于用户的授权策略，这里要添加用户;如果是基于角色的授权策略，这里要添加角色
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, name),
                    new Claim(JwtRegisteredClaimNames.Jti, user.FirstOrDefault().Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()),
                    new Claim(JwtRegisteredClaimNames.Name, user.FirstOrDefault().LoginName,ToString()),
                    new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(_requirement.Expiration.TotalSeconds).ToString()) };
                claims.AddRange(userRoles.Split(',').Select(s => new Claim(ClaimTypes.Role, s)));


                // ids4和jwt切换
                // jwt
                if (!Permissions.IsUseIds4)
                {
                    var data = await _roleModulePermissionServices.RoleModuleMaps();
                    var list = (from item in data
                                where item.IsDeleted == false
                                orderby item.Id
                                select new PermissionItem
                                {
                                    Url = item.Module?.LinkUrl,
                                    Role = item.Role?.Name.ObjToString(),
                                }).ToList();

                    _requirement.Permissions = list;
                }

                //TODO:jwt颁布有点乱，目前我用的是JwtToken这个类的静态方法
                //JwtHelper ..暂时没用它，但是复制了解析方法SerializeJwt到  JwtToken

                var token = JwtToken.BuildJwtToken(claims.ToArray(), _requirement, 1, "liuning");


                //测试解析
                var tokenModel = JwtToken.SerializeJwt($"{token.token}");


                return Success(token, "获取成功");
            }
            else
            {
                return Failed<TokenInfoViewModel>("认证失败");
            }
        }

        /// <summary>
        /// Minecraft玩家登录
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("MinecraftPlayerLogin")]
        public async Task<MessageModel<TokenInfoViewModel>> MinecraftPlayerLogin(string name = "", string pass = "")
        {
            //玩家不需要任何角色，直接登录 （OP作为用户对待）

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pass))
                return Failed<TokenInfoViewModel>("用户名或密码不能为空");
            pass = MD5Helper.MD5Encrypt32(pass);

          MinecraftPlayer playerInfo = await   _playerServices.LoginAsync(name, pass);

            var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, name),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()),
                   new Claim(JwtRegisteredClaimNames.Name, playerInfo.Name),
                    new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(_requirement.Expiration.TotalSeconds).ToString()) };

           

            var token = JwtToken.BuildJwtToken(claims.ToArray(), _requirement, 1, playerInfo.UUID);


            //测试解析
            var testTokenModel = JwtToken.SerializeJwt($"{token.token}");


            return Success(token, "登录成功");           
        }

        /// <summary>
        /// 测试 MD5 加密字符串
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Md5Password")]
        public string Md5Password(string password = "")
        {
            return MD5Helper.MD5Encrypt32(password);
        }

        /// <summary>
        /// swagger登录
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/Login/swgLogin")]
        public dynamic SwgLogin([FromBody] SwaggerLoginRequest loginRequest)
        {
            // 这里可以查询数据库等各种校验
            if (loginRequest?.name == "admin" && loginRequest?.pwd == "admin")
            {
                HttpContext.Session.SetString("swagger-code", "success");
                return new { result = true };
            }

            return new { result = false };
        }

    }

    public class SwaggerLoginRequest
    {
        public string name { get; set; }
        public string pwd { get; set; }
    }
}