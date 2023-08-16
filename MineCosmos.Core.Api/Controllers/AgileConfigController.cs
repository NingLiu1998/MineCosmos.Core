using MineCosmos.Core.Common.Helper;
using MineCosmos.Core.Controllers;
using MineCosmos.Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nacos.V2;
using System.Threading.Tasks;
using AgileConfig.Client.RegisterCenter;

namespace MineCosmos.Core.Api.Controllers
{
    /// <summary>
    /// AgileConfig服务管理 
    /// 原作者文章：https://www.cnblogs.com/kklldog/p/agileconfig-160.html
    /// </summary>
    [Produces("application/json")]
    [Route("api/[Controller]/[action]")]
    [Authorize(Permissions.Name)]
    public class AgileConfigController : BaseApiController
    {

        #region 变量

        /// <summary>
        /// IDiscoveryService
        /// </summary>
        private readonly IDiscoveryService _discoveryService;

        #endregion

        #region 重载
        /// <summary>
        /// 
        /// </summary>
        /// <param name="discoveryService"></param>
        public AgileConfigController(IDiscoveryService discoveryService)
        {
            _discoveryService = discoveryService;
        }

        #endregion


        /// <summary>
        /// 系统实例是否启动完成
        /// </summary>
        /// <returns></returns>
        [HttpGet]

        public MessageModel<string> CheckSystemStartFinish()
        {
            //********************* 用当前接口做基本健康检查 确定 基础服务 数据库 缓存都已正常启动*****
            // 然后再进行服务上线
            var data = new MessageModel<string>();
            // ***************  此处请求一下db 跟redis连接等 项目中简介 保证项目已全部启动
            data.success = true;
            data.msg = "SUCCESS";
            return data;

        }

        /// <summary>
        /// 获取Nacos 状态
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public MessageModel<ServiceInfo> GetStatus()
        {
            var currentIp = IpHelper.GetCurrentIp(null);
            var currentService = _discoveryService.Services.Find(a => a.Ip == currentIp);
            return Success(currentService);
        }

    }
}
