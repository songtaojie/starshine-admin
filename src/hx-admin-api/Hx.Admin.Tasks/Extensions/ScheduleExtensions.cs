// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Sdk.Core;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
namespace Hx.Admin.Tasks;
public static class ScheduleExtensions
{
    /// <summary>
    /// 扫描类型集合
    /// </summary>
    /// <param name="quartzOptions"></param>
    /// <returns></returns>
    public static IServiceCollectionQuartzConfigurator ScanToBuilders(this IServiceCollectionQuartzConfigurator quartzOptions)
    {
        var jobTypes = App.EffectiveTypes.Where(IsJobType);
        if (jobTypes.Any())
        {
            foreach (var jobtype in jobTypes)
            {
                var jobdetail = jobtype.GetCustomAttribute<JobDetailAttribute>();
                JobKey jobKey;
                if (jobdetail == null)
                {
                    jobKey = new JobKey(jobtype.FullName!);
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(jobdetail.JobId))
                        jobdetail.JobId = jobtype.FullName!;
                    jobKey = new JobKey(jobdetail.JobId, jobdetail.GroupName);
                }
                quartzOptions.AddJob(jobtype, jobKey,jobBuilder =>
                {
                    jobBuilder.WithDescription(jobdetail?.Description);
                });
                var jobtriggerlist = jobtype.GetCustomAttributes<TriggerAttribute>(true);
                if (jobtriggerlist.Any())
                {
                    int index = 0;
                    foreach (var jobtrigger in jobtriggerlist)
                    {
                        if (string.IsNullOrWhiteSpace(jobtrigger.TriggerId))
                            jobtrigger.TriggerId = $"{jobKey.Name}-trigger-{index}";
                        // create a trigger for the job
                        quartzOptions.AddTrigger(triggerBuilder =>
                        {
                            triggerBuilder.ForJob(jobKey)
                                .WithIdentity(jobtrigger.TriggerId, jobdetail.GroupName)
                                .WithDescription(jobtrigger.Description);
                            if (jobtrigger.StartNow)
                            {
                                triggerBuilder.StartNow();
                            }
                            else if (jobtrigger.RuntimeStartTime.HasValue)
                            {
                                triggerBuilder.StartAt(jobtrigger.RuntimeStartTime.Value);
                            }
                            if (jobtrigger.RuntimeEndTime.HasValue)
                            {
                                triggerBuilder.EndAt(jobtrigger.RuntimeEndTime.Value);
                            }
                            if (jobtrigger.TriggerType == TriggerTypeEnum.Corn && jobtrigger is CronTriggerAttribute cronTrigger)
                            {
                                triggerBuilder.WithCronSchedule(cronTrigger.Cron!);
                            }
                            else if (jobtrigger.TriggerType == TriggerTypeEnum.Simple && jobtrigger is PeriodTriggerAttribute periodTrigger)
                            {
                                var interval = jobtrigger.TriggerArgs?.FirstOrDefault() as long?;
                                if (interval.HasValue && interval > 100)
                                {
                                    triggerBuilder.WithSimpleSchedule(s =>
                                    {
                                        s.WithInterval(TimeSpan.FromMilliseconds(interval.Value)).RepeatForever();
                                    });
                                }
                            }
                        });
                        index++;
                    }
                }
            }
        }
        return quartzOptions;
    }


    /// <summary>
    /// 扫描类型集合
    /// </summary>
    /// <param name="quartzOptions"></param>
    /// <returns></returns>
    public static QuartzOptions ScanToBuilders(this QuartzOptions quartzOptions)
    {
        var jobTypes = App.EffectiveTypes.Where(IsJobType);
        if (jobTypes.Any())
        {
            foreach (var jobtype in jobTypes)
            {
                var jobDetailAttribute = jobtype.GetCustomAttribute<JobDetailAttribute>();
                JobKey jobKey;
                if (jobDetailAttribute == null)
                {
                    jobKey = new JobKey(jobtype.FullName!);
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(jobDetailAttribute.JobId))
                        jobDetailAttribute.JobId = jobtype.FullName!;
                    jobKey = new JobKey(jobDetailAttribute.JobId, jobDetailAttribute.GroupName);
                }
                quartzOptions.AddJob(jobtype, jobBuilder =>
                {
                    jobBuilder.WithIdentity(jobKey).WithDescription(jobDetailAttribute?.Description);
                });
                var jobtriggerlist = jobtype.GetCustomAttributes<TriggerAttribute>(true);
                if (jobtriggerlist.Any())
                {
                    int index = 0;
                    foreach (var jobtrigger in jobtriggerlist)
                    {
                        if (string.IsNullOrWhiteSpace(jobtrigger.TriggerId))
                            jobtrigger.TriggerId = $"{jobKey.Name}-trigger-{index}";
                        // create a trigger for the job
                        quartzOptions.AddTrigger(triggerBuilder =>
                        {
                            triggerBuilder.ForJob(jobKey)
                                .WithIdentity(jobtrigger.TriggerId, jobDetailAttribute.GroupName)
                                .WithDescription(jobtrigger.Description);
                            if (jobtrigger.StartNow)
                            {
                                triggerBuilder.StartNow();
                            }
                            else if (jobtrigger.RuntimeStartTime.HasValue)
                            {
                                triggerBuilder.StartAt(jobtrigger.RuntimeStartTime.Value);
                            }
                            if (jobtrigger.RuntimeEndTime.HasValue)
                            {
                                triggerBuilder.EndAt(jobtrigger.RuntimeEndTime.Value);
                            }
                            if (jobtrigger.TriggerType == TriggerTypeEnum.Corn && jobtrigger is CronTriggerAttribute cronTrigger)
                            {
                                triggerBuilder.WithCronSchedule(cronTrigger.Cron!);
                            }
                            else if (jobtrigger.TriggerType == TriggerTypeEnum.Simple && jobtrigger is PeriodTriggerAttribute periodTrigger)
                            {
                                var interval = jobtrigger.TriggerArgs?.FirstOrDefault() as long?;
                                if (interval.HasValue && interval > 100)
                                {
                                    triggerBuilder.WithSimpleSchedule(s =>
                                    {
                                        s.WithInterval(TimeSpan.FromMilliseconds(interval.Value)).RepeatForever();
                                    });
                                }
                            }
                        });
                        index++;
                    }
                }
            }
        }
        return quartzOptions;
    }

    /// <summary>
    /// 判断类型是否是 IJob 实现类型
    /// </summary>
    /// <param name="jobType">类型</param>
    /// <returns></returns>
    public static bool IsJobType(Type jobType)
    {
        if (!typeof(IJob).IsAssignableFrom(jobType) || jobType.IsInterface || jobType.IsAbstract)
        {
            return false;
        }

        return true;
    }
}

