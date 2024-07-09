// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// �绰/΢�ţ�song977601042

using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger(); 
try
{
   
    Log.Information("Starting web application");
    var builder = WebApplication.CreateBuilder(args);
    builder.ConfigureStarshineWebApp();
    builder.Services.AddAdminCoreService(builder.Configuration);
    builder.Logging.ClearProviders();
    builder.Host.UseSerilogSetup();
    var app = builder.Build();
    app.UseAdminCoreApp(builder.Environment);
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "�������쳣��ֹͣ");
}
finally
{
    // ȷ����Ӧ�ó����˳�֮ǰˢ�º�ֹͣ�ڲ���ʱ��/�߳�(����Linux�ϵķֶδ���)
    Log.CloseAndFlush();
}