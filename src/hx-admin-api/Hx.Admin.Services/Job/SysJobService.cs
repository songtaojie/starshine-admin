// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Admin.Models;
using Hx.Admin.IServices;
using Hx.Admin.Models.ViewModels.Job;
using Quartz;
using Hx.Admin.Tasks;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Qiniu.CDN;
using Quartz.Impl.AdoJobStore;
using Quartz.Spi;

namespace Hx.Admin.Services;

/// <summary>
/// 系统作业任务服务 🧩
/// </summary>
public class SysJobService : BaseService<QrtzJobDetails,int>, ISysJobService
{
    private readonly ISchedulerFactory _schedulerFactory;
    private readonly IDynamicJobCompiler _dynamicJobCompiler;
    private readonly ILogger _logger;
    public SysJobService(ISqlSugarRepository<QrtzJobDetails> jobDetailRep,
        ISchedulerFactory schedulerFactory,
        IDynamicJobCompiler dynamicJobCompiler,
        ILogger<SysJobService> logger) : base(jobDetailRep)
    {
        _schedulerFactory = schedulerFactory;
        _dynamicJobCompiler = dynamicJobCompiler;
        _logger = logger;
    }

    public async Task AddTriggerRecord(AddTriggerRecordInput addTriggerRecordInput)
    {
        var qrtzTriggerRecord = addTriggerRecordInput.Adapt<QrtzTriggerRecord>();
        await _rep.Context.Insertable(qrtzTriggerRecord).ExecuteCommandAsync();
    }

    /// <summary>
    /// 初始化数据库job到quartz
    /// </summary>
    /// <param name="quartzOptions"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task ScanDbJobToQuartz(QuartzOptions quartzOptions)
    {
        // 获取数据库所有通过脚本创建的作业
        try
        {
            var allDbScriptJobs = await _rep.AsQueryable().Where(u => u.CreateType != JobCreateTypeEnum.BuiltIn).ToListAsync();
            foreach (var dbDetail in allDbScriptJobs)
            {
                // 动态创建作业
                Type? jobType;
                JobKey? jobKey = null;
                switch (dbDetail.CreateType)
                {
                    case JobCreateTypeEnum.Script:
                        jobType = _dynamicJobCompiler.BuildJob(dbDetail.ScriptCode!);
                        if (jobType != null)
                        {
                            jobKey = new JobKey(dbDetail.JobName, dbDetail.JobGroup);
                            quartzOptions.AddJob(jobType, jobBuilder =>
                            {

                                jobBuilder.WithIdentity(jobKey)
                                    .WithDescription(dbDetail.Description)
                                    .StoreDurably(dbDetail.IsDurable)
                                    .RequestRecovery(dbDetail.RequestsRecovery);
                                if (dbDetail.JobData != null)
                                {
                                    var jobData = JsonSerializer.Deserialize<IDictionary<string, object>>(dbDetail.JobData);
                                    if(jobData != null) jobBuilder.SetJobData(new JobDataMap(jobData));
                                }
                                jobBuilder.DisallowConcurrentExecution(dbDetail.IsNonConcurrent)
                                        .PersistJobDataAfterExecution(dbDetail.IsUpdateData);

                            });
                        }
                        break;
                    //case JobCreateTypeEnum.Http:
                    //    jobType = typeof(HttpJob);
                    //    break;

                    default:
                        throw new NotSupportedException();
                }

                // 获取作业的所有数据库的触发器加入到作业中
                var dbTriggers = await _rep.Context.Queryable<QrtzTriggers>().Where(u => u.SchedulerName == dbDetail.SchedulerName && u.JobGroup == dbDetail.JobGroup && u.JobName == dbDetail.JobName).ToListAsync();
                dbTriggers.ForEach(dbTrigger =>
                {
                    quartzOptions.AddTrigger(async triggerBuilder =>
                    {
                        triggerBuilder.ForJob(jobKey!)
                                    .WithIdentity(dbTrigger.TriggerName, dbTrigger.TriggerGroup ?? dbTrigger.JobGroup)
                                    .WithDescription(dbTrigger.Description);
                        if (dbTrigger.StartTime > 0)
                        {
                            triggerBuilder.StartAt(new DateTimeOffset(dbTrigger.StartTime, TimeSpan.Zero));
                        }
                        if (dbTrigger.EndTime.HasValue && dbTrigger.EndTime > 0)
                        {
                            triggerBuilder.EndAt(new DateTimeOffset(dbTrigger.EndTime.Value, TimeSpan.Zero));
                        }
                        IOperableTrigger? trigger = null;
                        if (AdoConstants.TriggerTypeBlob.Equals(dbTrigger.TriggerType))
                        {
                            var blobTrigger = await _rep.Context.Queryable<QrtzBlobTriggers>()
                                .Where(u => u.SchedulerName == dbTrigger.SchedulerName && u.TriggerGroup == dbTrigger.TriggerGroup && u.TriggerName == dbTrigger.TriggerName)
                                .Select(u => new { u.BlobData })
                                .FirstAsync();
                            if (blobTrigger != null && blobTrigger.BlobData != null)
                            {
                                trigger = JsonSerializer.Deserialize<>
                            }
                        }
                        else
                        { 
                            
                        }
                        if (dbTrigger.TriggerType == "CRON")
                        {
                            var cronTrigger = await _rep.Context.Queryable<QrtzCronTriggers>().Where(u => u.TriggerGroup == dbTrigger.TriggerGroup && u.TriggerName == dbTrigger.TriggerName).FirstAsync();
                            if (cronTrigger != null)
                            {
                                triggerBuilder.WithCronSchedule(cronTrigger.CronExpression);
                            }
                        }
                        else
                        {
                            var simpleTrigger = await _rep.Context.Queryable<QrtzSimpleTriggers>().Where(u => u.TriggerGroup == dbTrigger.TriggerGroup && u.TriggerName == dbTrigger.TriggerName).FirstAsync();
                            if (simpleTrigger != null)
                            {
                                triggerBuilder.WithSimpleSchedule(s =>
                                {
                                    s.WithInterval(TimeSpan.FromMilliseconds(simpleTrigger.RepeatInterval));
                                    if (simpleTrigger.RepeatCount < 0)
                                    {
                                        s.RepeatForever();
                                    }
                                    else
                                    {
                                        s.WithRepeatCount(Convert.ToInt32(simpleTrigger.RepeatCount));
                                    }
                                });
                            }
                        }
                    });
                });
            }
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, "添加数据库任务调度失败");
        }
    }

    private IDictionary<string, object>? GetJobData(byte[] jobData)
    {
        return JsonSerializer.Deserialize<IDictionary<string, object>>(jobData);
    }


    public async Task<PagedListResult<PageJobDetailOutput>> PageJobDetail(PageJobDetailInput input)
    {
        var jobDetails = await _rep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.JobName), u => u.JobName!.Contains(input.JobName))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Description), u => u.Description!.Contains(input.Description))
            .Select<PageJobDetailOutput>()
            .ToPagedListAsync(input.Page, input.PageSize);

        await _rep.Context.ThenMapperAsync(jobDetails.Items, async r =>
        {
            r.JobTriggers = await _rep.Context.Queryable<QrtzTriggers>()
                .Where(u => u.SchedulerName == r.SchedulerName && u.JobGroup == r.JobGroup && u.JobName == r.JobName)
                .Select<PageJobTriggersOutput>()
                .ToListAsync();
        });

        // 提取中括号里面的参数值
        //var rgx = new Regex(@"(?i)(?<=\[)(.*)(?=\])");
        //foreach (var job in jobDetails.Items)
        //{
        //    foreach (var jobTrigger in job.JobTriggers)
        //    {
        //        jobTrigger.Args = rgx.Match(jobTrigger.Args ?? "").Value;
        //    }
        //}
        return jobDetails;
    }

    /// <summary>
    /// 添加作业 ⏰
    /// </summary>
    /// <returns></returns>
    public async Task AddJobDetail(AddJobDetailInput input)
    {
        var isExist = await _rep.IsAnyAsync(u => u.SchedulerName == input.SchedulerName && u.JobName == input.JobName && u.JobGroup == input.JobGroup);
        if (isExist)
            throw new UserFriendlyException();
        var dbDetail = input.Adapt<QrtzJobDetails>();
        var scheduler = await _schedulerFactory.GetScheduler();
        await scheduler.AddJob(BuildJobDetail(dbDetail), true);
        // 延迟一下等待持久化写入，再执行其他字段的更新
        await Task.Delay(500);
        await _rep.Context.Updateable<QrtzJobDetails>()
            .SetColumns(u => new QrtzJobDetails { CreateType = input.CreateType, ScriptCode = input.ScriptCode })
            .Where(u =>u.SchedulerName == input.SchedulerName && u.JobName == input.JobName && u.JobGroup == input.JobGroup)
            .ExecuteCommandAsync();
    }

    /// <summary>
    /// 更新作业 ⏰
    /// </summary>
    /// <returns></returns>
    public async Task UpdateJobDetail(UpdateJobDetailInput input)
    {
        var isExist = await _rep.IsAnyAsync(u => u.JobGroup == input.JobGroup && u.JobName == input.JobName && u.Id != input.Id);
        if (isExist)
            throw new UserFriendlyException($"组【{input.JobGroup}】已存在编号为【{input.JobName}】的作业");

        var dbDetail = await _rep.GetFirstAsync(u => u.Id == input.Id);
        if (dbDetail.JobName != input.JobName)
            throw new UserFriendlyException("禁止修改作业编号");
        var scheduler = await _schedulerFactory.GetScheduler();
        var oldScriptCode = dbDetail.ScriptCode; // 旧脚本代码
        input.Adapt(dbDetail);
        if (input.ScriptCode != oldScriptCode)
        {
            await scheduler.AddJob(BuildJobDetail(dbDetail), true);
        }
        // Tip: 假如这次更新有变更了 JobId，变更 JobId 后触发的持久化更新执行，会由于找不到 JobId 而更新不到数据
        // 延迟一下等待持久化写入，再执行其他字段的更新
        await Task.Delay(500);
        await _rep.UpdateAsync(dbDetail);
    }

    private IJobDetail BuildJobDetail(QrtzJobDetails dbDetail)
    {
        // 动态创建作业
        Type? jobType;
        switch (dbDetail.CreateType)
        {
            case JobCreateTypeEnum.Script when string.IsNullOrEmpty(dbDetail.ScriptCode):
                throw new UserFriendlyException("脚本代码不能为空");
            case JobCreateTypeEnum.Script:
                {
                    jobType = _dynamicJobCompiler.BuildJob(dbDetail.ScriptCode);
                    if (jobType == null)
                        throw new UserFriendlyException("脚本代码中的作业类需要实现IJob接口");
                    if (jobType.GetCustomAttributes(typeof(JobDetailAttribute), true).FirstOrDefault() is not JobDetailAttribute jobDetailAttribute)
                        throw new UserFriendlyException("脚本代码中的作业类，需要定义 [JobDetail] 特性");
                    if (jobDetailAttribute.JobId != dbDetail.JobName)
                        throw new UserFriendlyException("作业编号需要与脚本代码中的作业类 [JobDetail('jobId')] 一致");
                    break;
                }
            //case JobCreateTypeEnum.Http:
            //    jobType = typeof(HttpJob);
            //    break;

            default:
                throw new NotSupportedException();
        }
        JobBuilder jobBuilder = JobBuilder.Create(jobType);
        var jobKey = new JobKey(dbDetail.JobName, dbDetail.JobGroup);
        jobBuilder.WithIdentity(jobKey)
                .WithDescription(dbDetail.Description)
                .StoreDurably(dbDetail.IsDurable)
                .DisallowConcurrentExecution(!dbDetail.IsNonConcurrent);
        return jobBuilder.Build();
    }

    /// <summary>
    /// 删除作业 ⏰
    /// </summary>
    /// <returns></returns>
    public async Task DeleteJobDetail(DeleteJobDetailInput input)
    {
        var scheduler = await _schedulerFactory.GetScheduler();
        var jobKey = new JobKey(input.JobName, input.JobGroup);
        await scheduler.DeleteJob(jobKey);

        // 如果 _schedulerFactory 中不存在 JodId，则无法触发持久化，下面的代码确保作业和触发器能被删除
        await _rep.DeleteAsync(u => u.JobGroup == input.JobGroup && u.JobName == input.JobName);
        await _rep.Context.Deleteable<QrtzTriggers>()
            .Where(u => u.JobGroup == input.JobGroup && u.JobName == input.JobName)
            .ExecuteCommandAsync();
    }

    /// <summary>
    /// 获取触发器列表 ⏰
    /// </summary>
    public async Task<List<ListJobTriggerOutput>> GetJobTriggerList(ListJobTriggerInput input)
    {
        return await _rep.Context.Queryable<QrtzTriggers>()
            .WhereIF(!string.IsNullOrWhiteSpace(input.JobGroup), u => u.JobGroup.Contains(input.JobGroup))
            .WhereIF(!string.IsNullOrWhiteSpace(input.JobName), u => u.JobName.Contains(input.JobName))
            .Select<ListJobTriggerOutput>()
            .ToListAsync();
    }

    /// <summary>
    /// 添加触发器 ⏰
    /// </summary>
    /// <returns></returns>
    public async Task AddJobTrigger(AddJobTriggerInput input)
    {
        var isExist = await _rep.Context.Queryable<QrtzTriggers>()
            .AnyAsync(u => u.TriggerName == input.TriggerName && u.JobGroup == input.JobGroup);
        if (isExist)
            throw new UserFriendlyException($"已存在名称为【{input.TriggerName}】的触发器");

        var dbTrigger = input.Adapt<QrtzTriggers>();
        TriggerBuilder triggerBuilder = TriggerBuilder.Create();
        
        var jobKey = new JobKey(dbTrigger.JobName, dbTrigger.JobGroup);
        triggerBuilder.ForJob(jobKey)
                    .WithIdentity(dbTrigger.TriggerName, dbTrigger.TriggerGroup ?? dbTrigger.JobGroup)
                    .WithDescription(dbTrigger.Description);
        if (dbTrigger.StartTime > 0)
        {
            triggerBuilder.StartAt(new DateTimeOffset(dbTrigger.StartTime, TimeSpan.Zero));
        }
        if (dbTrigger.EndTime.HasValue && dbTrigger.EndTime > 0)
        {
            triggerBuilder.EndAt(new DateTimeOffset(dbTrigger.EndTime.Value, TimeSpan.Zero));
        }
        if (dbTrigger.TriggerType == "CRON")
        {
            var cronTrigger = await _rep.Context.Queryable<QrtzCronTriggers>().Where(u => u.TriggerGroup == dbTrigger.TriggerGroup && u.TriggerName == dbTrigger.TriggerName).FirstAsync();
            if (cronTrigger != null)
            {
                triggerBuilder.WithCronSchedule(cronTrigger.CronExpression);
            }
        }
        else
        {
            var simpleTrigger = await _rep.Context.Queryable<QrtzSimpleTriggers>().Where(u => u.TriggerGroup == dbTrigger.TriggerGroup && u.TriggerName == dbTrigger.TriggerName).FirstAsync();
            if (simpleTrigger != null)
            {
                triggerBuilder.WithSimpleSchedule(s =>
                {
                    s.WithInterval(TimeSpan.FromMilliseconds(simpleTrigger.RepeatInterval));
                    if (simpleTrigger.RepeatCount < 0)
                    {
                        s.RepeatForever();
                    }
                    else
                    {
                        s.WithRepeatCount(Convert.ToInt32(simpleTrigger.RepeatCount));
                    }
                });
            }
        }
        var scheduler = await _schedulerFactory.GetScheduler();
        await scheduler.ScheduleJob(triggerBuilder.Build());
    }

    /// <summary>
    /// 更新触发器 ⏰
    /// </summary>
    /// <returns></returns>
    public async Task UpdateJobTrigger(UpdateJobTriggerInput input)
    {
        var isExist = await _rep.Context.Queryable<QrtzTriggers>()
            .AnyAsync(u => u.TriggerName == input.TriggerName && u.JobGroup == input.JobGroup);
        if (isExist)
            throw new UserFriendlyException($"已存在名称为【{input.TriggerName}】的触发器");
        var dbTrigger = input.Adapt<QrtzTriggers>();
        TriggerBuilder triggerBuilder = TriggerBuilder.Create();

        var jobKey = new JobKey(dbTrigger.JobName, dbTrigger.JobGroup);
        triggerBuilder.ForJob(jobKey)
                    .WithIdentity(dbTrigger.TriggerName, dbTrigger.TriggerGroup ?? dbTrigger.JobGroup)
                    .WithDescription(dbTrigger.Description);
        if (dbTrigger.StartTime > 0)
        {
            triggerBuilder.StartAt(new DateTimeOffset(dbTrigger.StartTime, TimeSpan.Zero));
        }
        if (dbTrigger.EndTime.HasValue && dbTrigger.EndTime > 0)
        {
            triggerBuilder.EndAt(new DateTimeOffset(dbTrigger.EndTime.Value, TimeSpan.Zero));
        }
        if (dbTrigger.TriggerType == "CRON")
        {
            var cronTrigger = await _rep.Context.Queryable<QrtzCronTriggers>().Where(u => u.TriggerGroup == dbTrigger.TriggerGroup && u.TriggerName == dbTrigger.TriggerName).FirstAsync();
            if (cronTrigger != null)
            {
                triggerBuilder.WithCronSchedule(cronTrigger.CronExpression);
            }
        }
        else
        {
            var simpleTrigger = await _rep.Context.Queryable<QrtzSimpleTriggers>().Where(u => u.TriggerGroup == dbTrigger.TriggerGroup && u.TriggerName == dbTrigger.TriggerName).FirstAsync();
            if (simpleTrigger != null)
            {
                triggerBuilder.WithSimpleSchedule(s =>
                {
                    s.WithInterval(TimeSpan.FromMilliseconds(simpleTrigger.RepeatInterval));
                    if (simpleTrigger.RepeatCount < 0)
                    {
                        s.RepeatForever();
                    }
                    else
                    {
                        s.WithRepeatCount(Convert.ToInt32(simpleTrigger.RepeatCount));
                    }
                });
            }
        }
        var scheduler = await _schedulerFactory.GetScheduler();
        await scheduler.ScheduleJob(triggerBuilder.Build());
        //var jobTrigger = input.Adapt<SysJobTrigger>();
        //jobTrigger.Args = "[" + jobTrigger.Args + "]";

        //var scheduler = _schedulerFactory.GetJob(input.JobId);
        //scheduler?.UpdateTrigger(Triggers.Create(input.AssemblyName, input.TriggerType).LoadFrom(jobTrigger));
    }

    /// <summary>
    /// 删除触发器 ⏰
    /// </summary>
    /// <returns></returns>
    public async Task DeleteJobTrigger(DeleteJobTriggerInput input)
    {
        var scheduler = await _schedulerFactory.GetScheduler();
        await scheduler.UnscheduleJob(new TriggerKey(input.TriggerName, input.TriggerGroup));
        // 如果 _schedulerFactory 中不存在 JodId，则无法触发持久化，下行代码确保触发器能被删除
        //await _rep.Context.Deleteable<QrtzTriggers>()
        //    .Where(u => u.JobGroup == input.JobGroup && u.JobName == input.JobName && u.TriggerGroup == input.TriggerGroup && u.TriggerName == input.TriggerName)
        //    .ExecuteCommandAsync();
    }

    /// <summary>
    /// 暂停所有作业 ⏰
    /// </summary>
    /// <returns></returns>
    public async Task PauseAllJob()
    {
        var scheduler = await _schedulerFactory.GetScheduler();
        await scheduler.PauseAll();
    }

    /// <summary>
    /// 启动所有作业 ⏰
    /// </summary>
    /// <returns></returns>
    public async Task StartAllJob()
    {
        var scheduler = await _schedulerFactory.GetScheduler();
        await scheduler.ResumeAll();
    }

    /// <summary>
    /// 暂停作业 ⏰
    /// </summary>
    public async Task PauseJob(JobDetailInput input)
    {
        var scheduler = await _schedulerFactory.GetScheduler();
        await scheduler.PauseJob(new JobKey(input.JobName, input.JobGroup));
    }

    /// <summary>
    /// 启动作业 ⏰
    /// </summary>
    public async Task StartJob(JobDetailInput input)
    {
        var scheduler = await _schedulerFactory.GetScheduler();
        await scheduler.ResumeJob(new JobKey(input.JobName, input.JobGroup));
    }

    /// <summary>
    /// 暂停触发器 ⏰
    /// </summary>
    public async Task PauseTrigger(JobTriggerInput input)
    {
        var scheduler = await _schedulerFactory.GetScheduler();
        await scheduler.PauseTrigger(new TriggerKey(input.TriggerName, input.TriggerGroup));
    }

    /// <summary>
    /// 启动触发器 ⏰
    /// </summary>
    public async Task StartTrigger(JobTriggerInput input)
    {
        var scheduler = await _schedulerFactory.GetScheduler();
        await scheduler.ResumeTrigger(new TriggerKey(input.TriggerName, input.TriggerGroup));
    }

    /// <summary>
    /// 获取集群列表 ⏰
    /// </summary>
    public async Task<List<PageSchedulerStateOutput>> GetJobClusterList()
    {
        return await _rep.Context.Queryable<QrtzSchedulerState>()
            .Select<PageSchedulerStateOutput>()
            .ToListAsync();
    }

    /// <summary>
    /// 获取作业触发器运行记录分页列表 ⏰
    /// </summary>
    public async Task<PagedListResult<PageJobTriggerRecordOutput>> PageJobTriggerRecord(PageJobTriggerRecordInput input)
    {
        return await _rep.Context.Queryable<QrtzTriggerRecord>()
            .WhereIF(!string.IsNullOrWhiteSpace(input.JobName), u => u.JobName.Contains(input.JobName))
            .WhereIF(!string.IsNullOrWhiteSpace(input.TriggerName), u => u.TriggerName.Contains(input.TriggerName))
            .OrderByDescending(u => u.CreatedTime)
            .Select<PageJobTriggerRecordOutput>()
            .ToPagedListAsync(input.Page, input.PageSize);
    }
}