using System.ComponentModel.DataAnnotations;
using System.Security.Permissions;
using Microsoft.AspNetCore.Mvc;
using MineCosmos.Core.Api.Filter;
using MineCosmos.Core.IRepository.Base;
using MineCosmos.Core.IServices.Minecraft;
using MineCosmos.Core.Model.Models;
using Newtonsoft.Json.Linq;

namespace MineCosmos.Core.Controllers
{
    /// <summary>
    /// Minecraft插件接口
    /// </summary>
    [Produces("application/json")]
    [Route("api/mc/[controller]/[action]")]
    [ApiController]
    [ApiResponse]
    public class PlayerController : BaseApiController
    {
        readonly IBaseRepository<MinecraftPlayer> _mcPlayerRepostiory;
        readonly IWareHouseService _wareHouseService;
        public PlayerController(IBaseRepository<MinecraftPlayer> mcPlayerRepository, IWareHouseService wareHouseService)
        {
            _mcPlayerRepostiory = mcPlayerRepository;
            _wareHouseService = wareHouseService;

        }

        /// <summary>
        /// 测试接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public dynamic Get()
        {
            return new Dictionary<string, object>() {
               {"c1","123" },
               {"c13",new string []{"1","2","3" } },
                { "x12",new Dictionary<string, object>() {
               {"c2","123" },
               {"c3",new string []{"1","2","3" } },

           }}
           };
        }

        /// <summary>
        /// 获取玩家所有仓库包括物品的信息
        /// </summary>
        /// <param name="playerId">玩家ID</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<dynamic> GetPlayerWareHouse([Required(ErrorMessage = "缺少玩家ID")] int playerId) => await _wareHouseService.GetPlayerAllWareHouse(playerId);


    }
}