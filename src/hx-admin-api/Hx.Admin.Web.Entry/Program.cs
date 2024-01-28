using Hx.Admin.Serilog;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting web application");
    var builder = WebApplication.CreateBuilder(args);
    builder.ConfigureHxWebApp();
    builder.Services.AddAdminCoreService(builder.Configuration);
    builder.Host.UseSerilogSetup();
    var app = builder.Build();
    app.UseAdminCoreApp(builder.Environment);
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