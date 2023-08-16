using MineCosmos.Core.Common;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;

namespace MineCosmos.Core.Extensions
{
    /// <summary>
    /// Redis缓存 启动服务
    /// </summary>
    public static class RedisSetup
    {
        public static void AddRedisSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            //获取连接字符串
            string redisConfiguration = AppSettings.app(new string[] { "Redis", "ConnectionString" });
            RedisHelper.Initialization(new CSRedis.CSRedisClient(redisConfiguration));
        }
    }
}
