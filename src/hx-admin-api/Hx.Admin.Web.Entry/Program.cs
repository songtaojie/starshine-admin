using NLog;
using NLog.Web;
using System;


// 在构建主机之前，早期初始化NLog以允许启动和异常日志
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
try
{
    logger.Info("Starting web application");
    var builder = WebApplication.CreateBuilder(args);
    builder.ConfigureHxWebApp();
    builder.Services.AddAdminCoreService(builder.Configuration);
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
    var app = builder.Build();
    app.UseAdminCoreApp(builder.Environment);
    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "程序因异常而停止");
}
finally
{
    // 确保在应用程序退出之前刷新和停止内部计时器/线程(避免Linux上的分段错误)
    NLog.LogManager.Shutdown();
}