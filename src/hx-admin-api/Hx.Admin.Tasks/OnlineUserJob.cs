using Hx.Admin.Models;
using Hx.Sqlsugar.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Hx.Admin.Tasks;

/// <summary>
/// 清理在线用户作业任务
/// </summary>
[JobDetail("job_onlineUser", Description = "清理在线用户", GroupName = "default")]
[CronTrigger("trigger_onlineUser", Description = "清理在线用户")]
public class OnlineUserJob : IJob
{
    private readonly IServiceProvider _serviceProvider;

    public OnlineUserJob(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async System.Threading.Tasks.Task Execute(IJobExecutionContext context)
    {
        using var serviceScope = _serviceProvider.CreateScope();

        var rep = serviceScope.ServiceProvider.GetRequiredService<SqlSugarRepository<SysOnlineUser>>();
        await rep.Context.Deleteable<SysOnlineUser>().ExecuteCommandAsync();

        var originColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("【" + DateTime.Now + "】服务重启清空在线用户");
        Console.ForegroundColor = originColor;

    }

}