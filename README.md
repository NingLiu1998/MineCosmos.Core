

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

> 根据实际情况进行的修改调整

- #### 增加sqlsugar OnError 处理

- #### 移除自行封装的Wechat相关操作
- 添加 [DotNetCore.SKIT.FlurlHttpClient.Wechat](https://github.com/fudiwei/DotNetCore.SKIT.FlurlHttpClient.Wechat)
- 添加 [MrHuo.OAuth](https://github.com/mrhuo/MrHuo.OAuth)

- #### 缓存 Redis使用变动
- 不再封装一层redis使用，直接使用Csredis

- #### 移除了ES
- #### 移除了Apollo
- #### 移除了Consul
- #### 移除了建行聚合支付
