

### MineCosmos.Core

> MineCosmos 生态计划的核心后端

### MineCosmos.Core 项目介绍


### MineCosmos 生态

- [MineCosmos.Core](https://gitee.com/xintianyuehui_ln/MineCosmos.Core) - 核心后端
- [MineCosmos.Web](https://gitee.com/xintianyuehui_ln/minecraft-abcserver-web) 综合站点前端
- MineCosmos.Bot 基于Discord与Kook的机器人
- [MineCosmos.Bukkit](https://gitee.com/xintianyuehui_ln/minecraft-abcserver-plugin) 基于Bukkit的插件
-  基于farbric的模组 MineLink 支持实时语音的在线聊天（仿discord/kook）




### 对原Blog.Core的相关调整

> 根据实际情况进行的修改，包含修改原因和修改内容

- #### Wechat模块的变动 （TODO）
- 移除原生对接WeChat模块，考虑人手问题以及WeChat官方变动的频繁性，改为
- 改为使用 https://github.com/fudiwei/DotNetCore.SKIT.FlurlHttpClient.Wechat 开源库

- #### 缓存 Redis使用变动
- 不再封装一层redis使用，直接使用Csredis


- #### 移除了ES
