using MineCosmos.Core.Common;
using MineCosmos.Core.Common.Helper;
using MineCosmos.Core.Extensions.NacosConfig;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using AgileConfig.Client.RegisterCenter;
using System.Threading.Tasks;

namespace MineCosmos.Core.Extensions;



/// <summary>
/// AgileConfig
/// 
/// </summary>
public static class AgileConfigSetup
{
    public static void AddAgileConfigSetup(this IServiceCollection services, IConfiguration Configuration)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        // 在实际生产工作中 本地开发是不需要注册nacos的 所以根据环境变量去判断 
        // 比如 开发环境 dev  测试环境 test  生产 prod  只有这几种环境变量的时候才需要去注册nacos
        if (AppSettings.app(new string[] { "Startup", "AgileConfig", "Enabled" }).ObjToBool())
        {
           


        }
    }
}

