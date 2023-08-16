> 基础架构fork 老张的Blog.Core

## 已下是根据实际情况进行的修改，包含修改原因和修改内容

### Wechat模块的变动 （TODO）
- 移除原生对接WeChat模块，考虑人手问题以及WeChat官方变动的频繁性，改为
- 改为使用 https://github.com/fudiwei/DotNetCore.SKIT.FlurlHttpClient.Wechat 开源库

### 缓存 Redis使用变动

- 不再封装一层redis使用，直接使用Csredis
- 

### 移除了ES
