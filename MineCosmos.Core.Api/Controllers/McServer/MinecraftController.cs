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
    [Route("api/mc/[action]")]
    [ApiController]
    [ApiResponse]
    public class MinecraftController : BaseApiController
    {

        readonly IBaseRepository<MinecraftPlayer> _mcPlayerRepostiory;
        public MinecraftController(IBaseRepository<MinecraftPlayer> mcPlayerRepository)
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

        /// <summary>
        /// 接受MC服务器传递的物品数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<dynamic> PutItem(ReqMinecraftItemDto model)
        {

            bool hasPlayer = await _mcPlayerRepostiory.AnyAsync(a => a.UUID == model.UUID);
            if(hasPlayer)
            {
                MinecraftPlayer player = await _mcPlayerRepostiory.GetAsync(a => a.UUID == model.UUID);
                
                //TODO 存到玩家默认仓库，注意考虑仓库空间上限
            }
            else
            {
                //直接创建用户 TODO:单独service封装
                MinecraftPlayer player = new MinecraftPlayer()
                {
                    UUID = model.UUID,
                    Name = model.Source,
                    LastServerLoginTime = DateTime.Now
                };
                await _mcPlayerRepostiory.Add(player);
            }

            return @model.Source;
        }

        public class ReqMinecraftItemDto
        {
            public string Source { get; set; }
            public string UUID { get; set; }
            public string SourceVersion { get; set; }
            public string ApiKey { get; set; }
        }

        /// <summary>
        /// Kook绑定
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<dynamic> KookBind(MinecraftRegisteServerPlayerInfo model)
        {
            MinecraftPlayer player = await _mcPlayerRepostiory.GetAsync(a => a.Name == model.Name);
            if (player is not null)
            {
                player.LastServerLoginTime = DateTime.Now;
                await _mcPlayerRepostiory.Update(player);
            }
            else
            {
                player = new MinecraftPlayer()
                {
                    Name = model.Name,
                    UUID = model.UUID,
                    LastServerLoginTime = DateTime.Now,
                    JoinType = "通过服务器加入",
                };
                await _mcPlayerRepostiory.Add(player);
            }
            return Ok(player);
        }

        public class MinecraftRegisteServerPlayerInfo
        {
            public string Name { get; set; }
            public string UUID { get; set; }
        }

    }
}