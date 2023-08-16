using System.Security.Permissions;
using Microsoft.AspNetCore.Mvc;
using MineCosmos.Core.Api.Filter;
using MineCosmos.Core.IRepository.Base;
using MineCosmos.Core.Model.Models;
using Newtonsoft.Json.Linq;

namespace MineCosmos.Core.Controllers
{
    /// <summary>
    /// Minecraft插件接口
    /// </summary>
    [Produces("application/json")]
    [Route("api/mc/player/[action]")]
    [ApiController]
    [ApiResponse]
    public class PlayerController : BaseApiController
    {
        readonly IBaseRepository<MinecraftPlayer> _mcPlayerRepostiory;
        public PlayerController(IBaseRepository<MinecraftPlayer> mcPlayerRepository)
        {
            _mcPlayerRepostiory = mcPlayerRepository;
        }

        /// <summary>
        /// 健康检查接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }


    }
}