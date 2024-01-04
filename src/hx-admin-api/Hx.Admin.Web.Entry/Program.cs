using AspNetCoreRateLimit;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using System.Net.Mail;
using System.Net;
using Hx.Admin.Core;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger(); 

try
{
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);
   
    builder.Services.AddAdminCoreService(builder.Configuration);
    builder.Host.UseSerilog((context, services, configuration) => configuration
             .ReadFrom.Configuration(context.Configuration)
             .ReadFrom.Services(services)
             .Enrich.FromLogContext()
             .WriteTo.Console());
   
    var app = builder.Build();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}