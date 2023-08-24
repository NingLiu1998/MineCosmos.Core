using System.Threading.Tasks;
using MineCosmos.Core.IServices;
using MineCosmos.Core.Model;
using MineCosmos.Core.Model.Models;
using MineCosmos.Core.Model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mapster;

namespace MineCosmos.Core.Controllers
{
    /// <summary>
    /// 用户角色关系
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Permissions.Name)]
    public class UserRoleController : Controller
    {
        private readonly ISysUserInfoServices _sysUserInfoServices;
        private readonly IUserRoleServices _userRoleServices;
        private readonly IRoleServices _roleServices;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sysUserInfoServices"></param>
        /// <param name="userRoleServices"></param>
        /// <param name="mapper"></param>
        /// <param name="roleServices"></param>
        public UserRoleController(ISysUserInfoServices sysUserInfoServices, IUserRoleServices userRoleServices,  IRoleServices roleServices)
        {
            _sysUserInfoServices = sysUserInfoServices;
            _userRoleServices = userRoleServices;
            _roleServices = roleServices;
        }



        /// <summary>
        /// 新建用户
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="loginPwd"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<SysUserInfoDto>> AddUser(string loginName, string loginPwd)
        {
            var userInfo = await _sysUserInfoServices.SaveUserInfo(loginName, loginPwd);
            return new MessageModel<SysUserInfoDto>()
            {
                success = true,
                msg = "添加成功",
                response = userInfo.Adapt<SysUserInfoDto>() //_mapper.Map<SysUserInfoDto>(userInfo)
            };
        }

        /// <summary>
        /// 新建Role
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<Role>> AddRole(string roleName)
        {
            return new MessageModel<Role>()
            {
                success = true,
                msg = "添加成功",
                response = await _roleServices.SaveRole(roleName)
            };
        }

        /// <summary>
        /// 新建用户角色关系
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="rid"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<UserRole>> AddUserRole(int uid, int rid)
        {
            return new MessageModel<UserRole>()
            {
                success = true,
                msg = "添加成功",
                response = await _userRoleServices.SaveUserRole(uid, rid)
            };
        }




    }
}
