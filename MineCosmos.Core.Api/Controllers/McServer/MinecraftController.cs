using Mapster;
using Microsoft.AspNetCore.Mvc;
using MineCosmos.Core.Api.Filter;
using MineCosmos.Core.Common.Helper;
using MineCosmos.Core.IRepository.Base;
using MineCosmos.Core.IServices.Minecraft;
using MineCosmos.Core.Model.Models;
using MineCosmos.Core.Model.ViewModels.Minecraft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharpNBT;
using SharpNBT.SNBT;

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
        readonly IWareHouseService _wareHouseService;
        public MinecraftController(IBaseRepository<MinecraftPlayer> mcPlayerRepository, IWareHouseService wareHouseService)
        {
            _mcPlayerRepostiory = mcPlayerRepository;
            _wareHouseService = wareHouseService;
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
        public async Task<dynamic> PutItem([FromBody] ReqMinecraftItemDto model)
        {

            bool hasPlayer = await _mcPlayerRepostiory.AnyAsync(a => a.UUID == model.UUID);

            PlayerWareHouse playerWareHouseDefault = new()
            {
                Name = $"{model.Name}的默认仓库",
                Remark = "接受MC服务器传递的物品数据时，自动创建",
                Type = PlayerWareHouseTypeEnum.默认仓库,
                UpperLimit = 100
            };


            MinecraftPlayer player = new();

            if (hasPlayer)
            {
                player = await _mcPlayerRepostiory.GetAsync(a => a.UUID == model.UUID);
            }
            else
            {
                //直接创建用户 TODO:单独service封装
                player = new MinecraftPlayer()
                {
                    UUID = model.UUID,
                    Name = model.Name,
                    Pwd = MD5Helper.MD5Encrypt32(model.Name),
                    LastServerLoginTime = DateTime.Now
                };
                player.Id = await _mcPlayerRepostiory.Add(player);
            }
            playerWareHouseDefault.PlayerId = player.Id;
            PlayerWareHouse wareHouse = await _wareHouseService.AutoCreateDefaultWareHouseAsync(new PlayerWareHouseCreateDto()
            {
                WareHouse = playerWareHouseDefault,
                Items = new List<PlayerWareHouseItem>
                 {
                      new PlayerWareHouseItem (){
                       ItemData= model.Source,
                       ItemType = WareHouseItemTypeEnum.插件
                      }
                 }.Adapt<List<PlayerWareHouseItemDto>>()
            });





            return "成功";
        }

        /// <summary>
        /// 获取云仓库物品
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<dynamic> PullItem([FromBody]ReqMinecraftItemDto model)
        {
            var nbtStr = await _wareHouseService.GetWareHouseItemByPlayer(model.UUID, 1);

            #region 模拟
            //模拟环节
            //NBT转json
            //CompoundTag? tags = StringNbt.Parse(nbtStr);
            //Dictionary<string, object>? nbtDic = NbtHelper.TagToDic(tags, null);
            //string webJson = JsonConvert.SerializeObject(nbtDic);

            ////json转NBT
            //var dics = JsonConvert.DeserializeObject<Dictionary<string, object>>(webJson);
            //var Ntag = NbtHelper.DicToTag(dics).Create();
            //nbtStr = Ntag.Stringify();
            #endregion

            return nbtStr;

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