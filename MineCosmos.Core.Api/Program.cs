﻿
// 以下为asp.net 6.0的写法，如果用5.0，请看Program.five.cs文件
using Autofac;
using Autofac.Extensions.DependencyInjection;
using MineCosmos.Core;
using MineCosmos.Core.Common;
using MineCosmos.Core.Common.LogHelper;
using MineCosmos.Core.Common.Seed;
using MineCosmos.Core.Extensions;
using MineCosmos.Core.Extensions.Middlewares;
using MineCosmos.Core.Filter;
using MineCosmos.Core.Hubs;
using MineCosmos.Core.IServices;
using MineCosmos.Core.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;
using AgileConfig.Client;


/*
 * Mapster Github ：
 * https://github.com/MapsterMapper/Mapster
 * 无废话的Mapster一些中文文档：
 * https://www.cnblogs.com/qiqigou/p/13696669.html
 * https://github.com/rivenfx/Mapster-docs
 */

var builder = WebApplication.CreateBuilder(args);

// 1、配置host与容器
builder.Host
.UseServiceProviderFactory(new AutofacServiceProviderFactory())
.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule(new AutofacModuleRegister());
    builder.RegisterModule<AutofacPropertityModuleReg>();
})
.ConfigureLogging((hostingContext, builder) =>
{
    builder.AddFilter("System", LogLevel.Error);
    builder.AddFilter("Microsoft", LogLevel.Error);
    builder.SetMinimumLevel(LogLevel.Error);
    builder.AddLog4Net(Path.Combine(Directory.GetCurrentDirectory(), "Log4net.config"));
});
//.ConfigureAppConfiguration((hostingContext, config) =>
//{
//    config.Sources.Clear();
//    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
//    config.AddConfigurationApollo("appsettings.apollo.json");
//});

#if DEBUG

builder.Host.UseAgileConfig(new ConfigClient($"appsettings.agileconfig.debug.json"));

#else

builder.Host.UseAgileConfig(new ConfigClient($"appsettings.agileconfig.json"));

#endif



// 2、配置服务
builder.Services.AddSingleton(new AppSettings(builder.Configuration));

builder.Services.AddSingleton(new LogLock(builder.Environment.ContentRootPath));
builder.Services.AddUiFilesZipSetup(builder.Environment);

Permissions.IsUseIds4 = AppSettings.app(new string[] { "Startup", "IdentityServer4", "Enabled" }).ObjToBool();
Permissions.IsUseAuthing = AppSettings.app(new string[] { "Startup", "Authing", "Enabled" }).ObjToBool();
RoutePrefix.Name = AppSettings.app(new string[] { "AppSettings", "SvcName" }).ObjToString();

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services.AddMemoryCacheSetup();
builder.Services.AddRedisSetup();
builder.Services.AddSqlsugarSetup();
builder.Services.AddDbSetup();
builder.Services.AddCorsSetup();
builder.Services.AddMiniProfilerSetup();
builder.Services.AddSwaggerSetup();
builder.Services.AddJobSetup();
builder.Services.AddHttpContextSetup();
builder.Services.AddAppTableConfigSetup(builder.Environment);
builder.Services.AddHttpApi();
builder.Services.AddRabbitMQSetup();
builder.Services.AddKafkaSetup(builder.Configuration);
builder.Services.AddEventBusSetup();


builder.Services.AddAuthorizationSetup();
if (Permissions.IsUseIds4 || Permissions.IsUseAuthing)
{
    if (Permissions.IsUseIds4) builder.Services.AddAuthentication_Ids4Setup();
    else if (Permissions.IsUseAuthing) builder.Services.AddAuthentication_AuthingSetup();
}
else
{
    builder.Services.AddAuthentication_JWTSetup();
}

builder.Services.AddIpPolicyRateLimitSetup(builder.Configuration);
builder.Services.AddSignalR().AddNewtonsoftJsonProtocol();
builder.Services.AddScoped<UseServiceDIAttribute>();
builder.Services.Configure<KestrelServerOptions>(x => x.AllowSynchronousIO = true)
        .Configure<IISServerOptions>(x => x.AllowSynchronousIO = true);

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddHttpPollySetup();
builder.Services.AddControllers(o =>
{
    o.Filters.Add(typeof(GlobalExceptionsFilter));
    //o.Conventions.Insert(0, new GlobalRouteAuthorizeConvention());
    o.Conventions.Insert(0, new GlobalRoutePrefixFilter(new RouteAttribute(RoutePrefix.Name)));
})
.AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
    //options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
    options.SerializerSettings.Converters.Add(new StringEnumConverter());
})
//.AddFluentValidation(config =>
//{
//    //程序集方式添加验证
//    config.RegisterValidatorsFromAssemblyContaining(typeof(UserRegisterVoValidator));
//    //是否与MvcValidation共存
//    config.DisableDataAnnotationsValidation = true;
//})
;

builder.Services.AddEndpointsApiExplorer();

builder.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);


// 3、配置中间件
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    //app.UseHsts();
}

app.UseIpLimitMiddle();
app.UseRequestResponseLogMiddle();
app.UseRecordAccessLogsMiddle();
app.UseSignalRSendMiddle();
app.UseIpLogMiddle();
app.UseAllServicesMiddle(builder.Services);

app.UseSession();
app.UseSwaggerAuthorized();
app.UseSwaggerMiddle(() => Assembly.GetExecutingAssembly().GetManifestResourceStream("MineCosmos.Core.Api.index.html"));

app.UseCors(AppSettings.app(new string[] { "Startup", "Cors", "PolicyName" }));
DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
defaultFilesOptions.DefaultFileNames.Clear();
defaultFilesOptions.DefaultFileNames.Add("index.html");
app.UseDefaultFiles(defaultFilesOptions);
app.UseStaticFiles();
app.UseCookiePolicy();
app.UseStatusCodePages();
app.UseRouting();

if (builder.Configuration.GetValue<bool>("AppSettings:UseLoadTest"))
{
    app.UseMiddleware<ByPassAuthMiddleware>();
}
app.UseAuthentication();
app.UseAuthorization();
app.UseMiniProfilerMiddleware();
//app.UseExceptionHandlerMidd();


//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllerRoute(
//        name: "default",
//        pattern: "{controller=Home}/{action=Index}/{id?}");

//    endpoints.MapHub<ChatHub>("/api2/chatHub");
//});
//改为顶级路由注册,参阅：https://learn.microsoft.com/zh-cn/aspnet/core/diagnostics/asp0014?view=aspnetcore-7.0

app.MapControllers();
app.MapControllerRoute(name: "default",pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHub<ChatHub>("/api2/chatHub");


var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
var myContext = scope.ServiceProvider.GetRequiredService<MyContext>();
var tasksQzServices = scope.ServiceProvider.GetRequiredService<ITasksQzServices>();
var schedulerCenter = scope.ServiceProvider.GetRequiredService<ISchedulerCenter>();
var lifetime = scope.ServiceProvider.GetRequiredService<IHostApplicationLifetime>();
app.UseSeedDataMiddle(myContext, builder.Environment.WebRootPath);
app.UseQuartzJobMiddleware(tasksQzServices, schedulerCenter);
app.ConfigureEventBus();

// 4、运行
app.Run();
