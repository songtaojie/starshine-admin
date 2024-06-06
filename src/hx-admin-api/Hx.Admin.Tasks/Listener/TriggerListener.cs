// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.Extensions.Logging;
using Quartz.Listener;
using Quartz;
using Hx.Admin.Models.ViewModels.Job;
using Quartz.Impl.AdoJobStore;
using Microsoft.Extensions.DependencyInjection;
using Hx.Admin.IServices;

namespace Hx.Admin.Tasks.Listener;
public class TriggerListener : TriggerListenerSupport
{
    private readonly ILogger _logger;
    private readonly IServiceProvider _serviceProvider;
    public TriggerListener(ILogger<TriggerListener> logger, 
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public override string Name => "Trigger Listener";

    public override Task TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode, CancellationToken cancellationToken = default)
    {
        var addTriggerRecordInput = new AddTriggerRecordInput
        {
            SchedulerName = context.Scheduler.SchedulerName,
            JobGroup = context.JobDetail.Key.Group,
            JobName = context.JobDetail.Key.Name,
            TriggerGroup = trigger.Key.Group,
            TriggerName = trigger.Key.Name,
            PrevFireTime = trigger.GetPreviousFireTimeUtc()?.UtcTicks,
            NextFireTime = trigger.GetNextFireTimeUtc()?.UtcTicks,
            ElapsedTime = Convert.ToDecimal(context.JobRunTime.TotalMilliseconds),
            Priority = trigger.Priority,
            TriggerType = GetTriggerType(trigger.GetType().Name),
            TriggerState = GetTriggerState(triggerInstructionCode)
        };
        Task.Run(async() =>
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var sysJobService = scope.ServiceProvider.GetRequiredService<ISysJobService>();
                await sysJobService.AddTriggerRecord(addTriggerRecordInput);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "添加任务执行记录失败");
            }
        }, cancellationToken);
        
        return base.TriggerComplete(trigger, context, triggerInstructionCode, cancellationToken);
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
            SchedulerInstruction.NoInstruction or SchedulerInstruction.SetAllJobTriggersComplete or SchedulerInstruction.SetTriggerComplete => "COMPLETE",
            SchedulerInstruction.SetAllJobTriggersError or SchedulerInstruction.SetTriggerError => "ERROR",
            _ => string.Empty
        };
    }
}