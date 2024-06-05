// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Admin.IServices;
using Hx.Admin.Models.ViewModels.Job;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl.AdoJobStore;
using Quartz.Listener;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.ComponentTCBBatchUpdateTriggersRequest.Types;

namespace Hx.Admin.Tasks.Listener;
public class JobDetailsListener: JobListenerSupport
{
    private readonly ILogger<JobDetailsListener> logger;
    private readonly IServiceProvider _serviceProvider;
    public JobDetailsListener(ILogger<JobDetailsListener> logger, 
        IServiceProvider serviceProvider)
    {
        this.logger = logger;
        _serviceProvider = serviceProvider;
    }

    public override string Name => "Job Listener";

    public override Task JobWasExecuted(IJobExecutionContext context, JobExecutionException? jobException, CancellationToken cancellationToken = default)
    {
        var addTriggerRecordInput = new AddTriggerRecordInput
        {
            SchedulerName = context.Scheduler.SchedulerName,
            JobGroup = context.JobDetail.Key.Group,
            JobName = context.JobDetail.Key.Name,
            TriggerGroup = context.Trigger.Key.Group,
            TriggerName = context.Trigger.Key.Name,
            PrevFireTime = context.Trigger.GetPreviousFireTimeUtc()?.UtcTicks,
            NextFireTime = context.Trigger.GetNextFireTimeUtc()?.UtcTicks,
            ElapsedTime = Convert.ToInt64(context.JobRunTime.TotalMilliseconds),
            Priority = context.Trigger.Priority,
            TriggerType = GetTriggerType(context.Trigger.GetType().Name),
            TriggerState = GetTriggerState(triggerInstructionCode)
        };
        Task.Run(() =>
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var sysJobService = scope.ServiceProvider.GetRequiredService<ISysJobService>();
                await sysJobService.AddTriggerRecord(new AddTriggerRecordInput
                {
                    SchedulerName = context.Scheduler.SchedulerName,
                    JobGroup = context.JobDetail.Key.Group,
                    JobName = context.JobDetail.Key.Name,
                    TriggerGroup = trigger.Key.Group,
                    TriggerName = trigger.Key.Name,
                    PrevFireTime = trigger.GetPreviousFireTimeUtc()?.UtcTicks,
                    NextFireTime = trigger.GetNextFireTimeUtc()?.UtcTicks,
                    ElapsedTime = Convert.ToInt64(context.JobRunTime.TotalMilliseconds),
                    Priority = trigger.Priority,
                    TriggerType = GetTriggerType(trigger.GetType().Name),
                    TriggerState = GetTriggerState(triggerInstructionCode)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "添加任务执行记录失败");
            }
        });
        return base.JobWasExecuted(context, jobException, cancellationToken);
    }

    private async Task AddRecord()
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var sysJobService = scope.ServiceProvider.GetRequiredService<ISysJobService>();
            await sysJobService.AddTriggerRecord(new AddTriggerRecordInput
            {
                SchedulerName = context.Scheduler.SchedulerName,
                JobGroup = context.JobDetail.Key.Group,
                JobName = context.JobDetail.Key.Name,
                TriggerGroup = trigger.Key.Group,
                TriggerName = trigger.Key.Name,
                PrevFireTime = trigger.GetPreviousFireTimeUtc()?.UtcTicks,
                NextFireTime = trigger.GetNextFireTimeUtc()?.UtcTicks,
                ElapsedTime = Convert.ToInt64(context.JobRunTime.TotalMilliseconds),
                Priority = trigger.Priority,
                TriggerType = GetTriggerType(trigger.GetType().Name),
                TriggerState = GetTriggerState(triggerInstructionCode)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "添加任务执行记录失败");
        }
    }

    private string GetTriggerType(string triggerTypeName)
    {

        if (triggerTypeName.Contains("SIMPLE", StringComparison.InvariantCultureIgnoreCase))
        {
            return AdoConstants.TriggerTypeSimple;
        }
        else if (triggerTypeName.Contains("CRON", StringComparison.InvariantCultureIgnoreCase))
        {
            return AdoConstants.TriggerTypeCron;
        }
        else if (triggerTypeName.Contains("CalendarInterval", StringComparison.InvariantCultureIgnoreCase))
        {
            return AdoConstants.TriggerTypeCalendarInterval;
        }
        else if (triggerTypeName.Contains("DailyTimeInterval", StringComparison.InvariantCultureIgnoreCase))
        {
            return AdoConstants.TriggerTypeDailyTimeInterval;
        }
        return AdoConstants.TriggerTypeBlob;
    }


    private string GetTriggerState(SchedulerInstruction triggerInstructionCode)
    {
        return triggerInstructionCode switch
        {
            SchedulerInstruction.SetAllJobTriggersComplete or SchedulerInstruction.SetTriggerComplete => "COMPLETE",
            SchedulerInstruction.SetAllJobTriggersError or SchedulerInstruction.SetTriggerError => "ERROR",
            _ => string.Empty
        };
    }
}
