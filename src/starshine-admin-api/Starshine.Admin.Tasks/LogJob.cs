// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042


using Quartz;

namespace Starshine.Admin.Tasks;
/// <summary>
/// 清理日志作业任务
/// </summary>
[JobDetail("job_log", Description = "清理操作日志", GroupName = "default")]
[PeriodSeconds(1,TriggerId = "trigger_log", Description = "清理操作日志",StartNow =true)]
[DisallowConcurrentExecution]
public class LogJob : IJob
{
    private readonly IServiceProvider _serviceProvider;

    public LogJob(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Execute(IJobExecutionContext context)
    {

        await Task.CompletedTask;
        //using var serviceScope = _serviceProvider.CreateScope();
        //var logVisRep = serviceScope.ServiceProvider.GetRequiredService<ISqlSugarRepository<SysLogVis>>();
        //var logOpRep = serviceScope.ServiceProvider.GetRequiredService<ISqlSugarRepository<SysLogOp>>();
        //var logDiffRep = serviceScope.ServiceProvider.GetRequiredService<ISqlSugarRepository<SysLogDiff>>();

        //var daysAgo = 30; // 删除30天以前
        //await logVisRep.Context.Deleteable<SysLogVis>().Where(u => u.CreateTime < DateTime.Now.AddDays(-daysAgo)).ExecuteCommandAsync(context.CancellationToken); // 删除访问日志
        //await logOpRep.Context.Deleteable<SysLogVis>().Where(u => u.CreateTime < DateTime.Now.AddDays(-daysAgo)).ExecuteCommandAsync(context.CancellationToken); // 删除操作日志
        //await logDiffRep.Context.Deleteable<SysLogVis>().Where(u => u.CreateTime < DateTime.Now.AddDays(-daysAgo)).ExecuteCommandAsync(context.CancellationToken); // 删除差异日志
    }
}