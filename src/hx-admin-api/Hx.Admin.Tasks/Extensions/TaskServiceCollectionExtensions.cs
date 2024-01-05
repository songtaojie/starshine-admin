// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz.AspNetCore;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hx.Admin.Tasks;
using Hx.Sdk.Core;
using Hx.Common.Extensions;
using System.Reflection;
using Hx.Admin.Core;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Task服务注册
/// </summary>
public static class TaskServiceCollectionExtensions
{
    public static IServiceCollection AddQuartzService(this IServiceCollection services, IConfiguration configuration)
    {

        var jobTypes = App.EffectiveTypes.Where(t => typeof(IJob).IsAssignableFrom(t));

        services.AddQuartz(q =>
        {
            // base Quartz scheduler, job and trigger configuration
            if (jobTypes.Any())
            {
                foreach (var jobType in jobTypes)
                {
                    var jobDetail = jobType.GetCustomAttribute<JobDetailAttribute>();
                    if (jobDetail == null) continue;
                    if(string.IsNullOrWhiteSpace(jobDetail.JobId))
                        jobDetail.JobId = jobType.FullName!;
                    // Create a "key" for the job
                    var jobKey = new JobKey(jobDetail.JobId);
                    // Register the job with the DI container
                    q.AddJob<LogJob>(opts => opts.WithIdentity(jobKey));
                    var jobTriggerList = jobType.GetCustomAttributes<JobTriggerAttribute>();
                    if (jobTriggerList.Any())
                    {
                        int index = 0;
                        foreach (var jobTrigger in jobTriggerList)
                        {
                            if (string.IsNullOrWhiteSpace(jobTrigger.TriggerId))
                                jobTrigger.TriggerId = $"{jobDetail.JobId}-trigger-{index}";
                            // Create a trigger for the job
                            q.AddTrigger(opts =>
                            {
                                opts.ForJob(jobKey)
                                    .WithIdentity(jobTrigger.TriggerId);
                                if (jobTrigger.StartNow)
                                    opts.StartNow();
                                var configCron = configuration[$"Quartz:{jobType.Name}"];
                                if (!string.IsNullOrWhiteSpace(configCron))
                                {
                                    opts.WithCronSchedule(configCron);
                                }
                                else if(!string.IsNullOrWhiteSpace(jobTrigger.Cron))
                                {
                                    opts.WithCronSchedule(jobTrigger.Cron);
                                }

                            }); // run every 5 seconds
                            index++;
                        }
                    }
                    else
                    {
                        // Create a trigger for the job
                        q.AddTrigger(opts =>
                        {
                            opts .ForJob(jobKey).StartNow().WithIdentity($"{jobDetail.JobId}-trigger");
                            var configCron = configuration[$"Quartz:{jobType.Name}"];
                            if (!string.IsNullOrWhiteSpace(configCron))
                            {
                                opts.WithCronSchedule(configCron);
                            }
                        }); 
                    }
                }
            }
           
        });

        // ASP.NET Core hosting
        services.AddQuartzServer(options =>
        {
            // when shutting down we want jobs to complete gracefully
            options.WaitForJobsToComplete = true;
        });
        return services;
    }
}
