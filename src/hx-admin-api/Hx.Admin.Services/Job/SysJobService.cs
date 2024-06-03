// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Admin.Models;
using Hx.Admin.IServices;
using Hx.Admin.Models.ViewModels.Job;
using Quartz;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using Microsoft.Extensions.Options;
using System.Collections;
using Hx.Admin.Tasks;

namespace Hx.Admin.Services;

/// <summary>
/// 系统作业任务服务 🧩
/// </summary>
public class SysJobService : ISysJobService//BaseService<QrtzJobDetails,int>, ISysJobService
{
    private readonly ISchedulerFactory _schedulerFactory;
    private readonly IDynamicJobCompiler _dynamicJobCompiler;
    public SysJobService(ISqlSugarRepository<QrtzJobDetails> jobDetailRep,
        ISchedulerFactory schedulerFactory,
        IDynamicJobCompiler dynamicJobCompiler) //: base(jobDetailRep)
    {
        _schedulerFactory = schedulerFactory;
        _dynamicJobCompiler = dynamicJobCompiler;
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
            await Task.Delay(500);
            return;
            //var allDbScriptJobs = await _rep.AsQueryable().Where(u => u.CreateType != JobCreateTypeEnum.BuiltIn).ToListAsync();
            //foreach (var dbDetail in allDbScriptJobs)
            //{
            //    // 动态创建作业
            //    Type? jobType;
            //    JobKey? jobKey = null;
            //    switch (dbDetail.CreateType)
            //    {
            //        case JobCreateTypeEnum.Script:
            //            jobType = _dynamicJobCompiler.BuildJob(dbDetail.ScriptCode!);
            //            if (jobType != null)
            //            {
            //                jobKey = new JobKey(dbDetail.JobName, dbDetail.JobGroup);
            //                quartzOptions.AddJob(jobType, jobBuilder =>
            //                {

            //                    jobBuilder.WithIdentity(jobKey)
            //                        .WithDescription(dbDetail.Description);
            //                    jobBuilder.StoreDurably(dbDetail.IsDurable);
            //                    jobBuilder.DisallowConcurrentExecution(!dbDetail.IsNonConcurrent);
            //                });
            //            }
            //            break;
            //        //case JobCreateTypeEnum.Http:
            //        //    jobType = typeof(HttpJob);
            //        //    break;

            //        default:
            //            throw new NotSupportedException();
            //    }

            //    // 获取作业的所有数据库的触发器加入到作业中
            //    var dbTriggers = await _rep.Context.Queryable<QrtzTriggers>().Where(u => u.SchedulerName == dbDetail.SchedulerName && u.JobGroup == dbDetail.JobGroup && u.JobName == dbDetail.JobName).ToListAsync();
            //    dbTriggers.ForEach(dbTrigger =>
            //    {
            //        quartzOptions.AddTrigger(async triggerBuilder =>
            //        {
            //            triggerBuilder.ForJob(jobKey!)
            //                        .WithIdentity(dbTrigger.TriggerName, dbTrigger.TriggerGroup ?? dbTrigger.JobGroup)
            //                        .WithDescription(dbTrigger.Description);
            //            if (dbTrigger.StartTime > 0)
            //            {
            //                triggerBuilder.StartAt(new DateTimeOffset(dbTrigger.StartTime, TimeSpan.Zero));
            //            }
            //            if (dbTrigger.EndTime.HasValue && dbTrigger.EndTime > 0)
            //            {
            //                triggerBuilder.EndAt(new DateTimeOffset(dbTrigger.EndTime.Value, TimeSpan.Zero));
            //            }
            //            if (dbTrigger.TriggerType == "CRON")
            //            {
            //                var cronTrigger = await _rep.Context.Queryable<QrtzCronTriggers>().Where(u => u.TriggerGroup == dbTrigger.TriggerGroup && u.TriggerName == dbTrigger.TriggerName).FirstAsync();
            //                if (cronTrigger != null)
            //                {
            //                    triggerBuilder.WithCronSchedule(cronTrigger.CronExpression);
            //                }
            //            }
            //            else
            //            {
            //                var simpleTrigger = await _rep.Context.Queryable<QrtzSimpleTriggers>().Where(u => u.TriggerGroup == dbTrigger.TriggerGroup && u.TriggerName == dbTrigger.TriggerName).FirstAsync();
            //                if (simpleTrigger != null)
            //                {
            //                    triggerBuilder.WithSimpleSchedule(s =>
            //                    {
            //                        s.WithInterval(TimeSpan.FromMilliseconds(simpleTrigger.RepeatInterval));
            //                        if (simpleTrigger.RepeatCount < 0)
            //                        {
            //                            s.RepeatForever();
            //                        }
            //                        else
            //                        {
            //                            s.WithRepeatCount(Convert.ToInt32(simpleTrigger.RepeatCount));
            //                        }
            //                    });
            //                }
            //            }
            //        });
            //    });
            //}
        }
        catch (Exception ex) 
        {
            
        }
    }

    //public async Task<PagedListResult<PageJobDetailOutput>> PageJobDetail(PageJobDetailInput input)
    //{
    //    var jobDetails = await _rep.AsQueryable()
    //        .WhereIF(!string.IsNullOrWhiteSpace(input.JobName), u => u.JobName.Contains(input.JobName))
    //        .WhereIF(!string.IsNullOrWhiteSpace(input.Description), u => u.Description!.Contains(input.Description))
    //        .Select<PageJobDetailOutput>()
    //        .ToPagedListAsync(input.Page, input.PageSize);

    //    await _rep.Context.ThenMapperAsync(jobDetails.Items, async r =>
    //    {
    //        r.JobTriggers = await _rep.Context.Queryable<QrtzTriggers>()
    //            .Where(u => u.SchedulerName == r.SchedulerName && u.JobGroup == r.JobGroup && u.JobName == r.JobName)
    //            .Select<PageJobTriggersOutput>()
    //            .ToListAsync();
    //    });

    //    // 提取中括号里面的参数值
    //    //var rgx = new Regex(@"(?i)(?<=\[)(.*)(?=\])");
    //    //foreach (var job in jobDetails.Items)
    //    //{
    //    //    foreach (var jobTrigger in job.JobTriggers)
    //    //    {
    //    //        jobTrigger.Args = rgx.Match(jobTrigger.Args ?? "").Value;
    //    //    }
    //    //}
    //    return jobDetails;
    //}

    ///// <summary>
    ///// 添加作业 ⏰
    ///// </summary>
    ///// <returns></returns>
    //public async Task AddJobDetail(AddJobDetailInput input)
    //{
    //    var isExist = await _rep.IsAnyAsync(u =>u.SchedulerName == input.SchedulerName && u.JobName == input.JobName && u.JobGroup == input.JobGroup);
    //    if (isExist)
    //        throw new UserFriendlyException();

    //    // 动态创建作业
    //    Type? jobType;
    //    switch (input.CreateType)
    //    {
    //        case JobCreateTypeEnum.Script when string.IsNullOrEmpty(input.ScriptCode):
    //            throw new UserFriendlyException("脚本代码不能为空");
    //        case JobCreateTypeEnum.Script:
    //            {
    //                jobType = _dynamicJobCompiler.BuildJob(input.ScriptCode);
    //                if (jobType == null)
    //                    throw new UserFriendlyException("脚本代码中的作业类需要实现IJob接口");
    //                if (jobType.GetCustomAttributes(typeof(JobDetailAttribute),true).FirstOrDefault() is not JobDetailAttribute jobDetailAttribute)
    //                    throw new UserFriendlyException("脚本代码中的作业类，需要定义 [JobDetail] 特性");
    //                if (jobDetailAttribute.JobId != input.JobName)
    //                    throw new UserFriendlyException("作业编号需要与脚本代码中的作业类 [JobDetail('jobId')] 一致");
    //                break;
    //            }
    //        //case JobCreateTypeEnum.Http:
    //        //    jobType = typeof(HttpJob);
    //        //    break;

    //        default:
    //            throw new NotSupportedException();
    //    }
    //    var scheduler = await _schedulerFactory.GetScheduler();
    //    var dbDetail = input.Adapt<QrtzJobDetails>();
    //    JobBuilder jobBuilder = JobBuilder.Create(jobType);
    //    var jobKey = new JobKey(dbDetail.JobName, dbDetail.JobGroup);
    //    jobBuilder.WithIdentity(jobKey)
    //            .WithDescription(dbDetail.Description)
    //            .StoreDurably(dbDetail.IsDurable)
    //            .DisallowConcurrentExecution(!dbDetail.IsNonConcurrent);
    //    await scheduler.AddJob(jobBuilder.Build(),true);
    //    // 延迟一下等待持久化写入，再执行其他字段的更新
    //    await Task.Delay(500);
    //    //await _rep.Context.Updateable<QrtzJobDetails>()
    //    //    .SetColumns(u => new QrtzJobDetails { CreateType = input.CreateType, ScriptCode = input.ScriptCode })
    //    //    .Where(u => u.JobName == input.JobName)
    //    //    .ExecuteCommandAsync();
    //}

    ///// <summary>
    ///// 更新作业 ⏰
    ///// </summary>
    ///// <returns></returns>
    //public async Task UpdateJobDetail(UpdateJobDetailInput input)
    //{
    //    var isExist = await _sysJobDetailRep.IsAnyAsync(u => u.JobId == input.JobId && u.Id != input.Id);
    //    if (isExist)
    //        throw Oops.Oh(ErrorCodeEnum.D1006);

    //    var sysJobDetail = await _sysJobDetailRep.GetFirstAsync(u => u.Id == input.Id);
    //    if (sysJobDetail.JobId != input.JobId)
    //        throw Oops.Oh(ErrorCodeEnum.D1704);
    //   var s =  await _schedulerFactory.GetScheduler();
    //    s.TriggerJob
    //    var scheduler = _schedulerFactory.GetJob(sysJobDetail.JobId);
    //    var oldScriptCode = sysJobDetail.ScriptCode; // 旧脚本代码
    //    input.Adapt(sysJobDetail);

    //    if (input.CreateType == JobCreateTypeEnum.Script)
    //    {
    //        if (string.IsNullOrEmpty(input.ScriptCode))
    //            throw Oops.Oh(ErrorCodeEnum.D1701);

    //        if (input.ScriptCode != oldScriptCode)
    //        {
    //            // 动态创建作业
    //            var jobType = _dynamicJobCompiler.BuildJob(input.ScriptCode);

    //            if (jobType.GetCustomAttributes(typeof(JobDetailAttribute)).FirstOrDefault() is not JobDetailAttribute jobDetailAttribute)
    //                throw Oops.Oh(ErrorCodeEnum.D1702);
    //            if (jobDetailAttribute.JobId != input.JobId)
    //                throw Oops.Oh(ErrorCodeEnum.D1703);

    //            scheduler?.UpdateDetail(JobBuilder.Create(jobType).LoadFrom(sysJobDetail).SetJobType(jobType));
    //        }
    //    }
    //    else
    //    {
    //        scheduler?.UpdateDetail(scheduler.GetJobBuilder().LoadFrom(sysJobDetail));
    //    }

    //    // Tip: 假如这次更新有变更了 JobId，变更 JobId 后触发的持久化更新执行，会由于找不到 JobId 而更新不到数据
    //    // 延迟一下等待持久化写入，再执行其他字段的更新
    //    await Task.Delay(500);
    //    await _sysJobDetailRep.UpdateAsync(sysJobDetail);
    //}

    ///// <summary>
    ///// 删除作业 ⏰
    ///// </summary>
    ///// <returns></returns>
    //[ApiDescriptionSettings(Name = "DeleteJobDetail"), HttpPost]
    //[DisplayName("删除作业")]
    //public async Task DeleteJobDetail(DeleteJobDetailInput input)
    //{
    //    _schedulerFactory.RemoveJob(input.JobId);

    //    // 如果 _schedulerFactory 中不存在 JodId，则无法触发持久化，下面的代码确保作业和触发器能被删除
    //    await _sysJobDetailRep.DeleteAsync(u => u.JobId == input.JobId);
    //    await _sysJobTriggerRep.DeleteAsync(u => u.JobId == input.JobId);
    //}

    ///// <summary>
    ///// 获取触发器列表 ⏰
    ///// </summary>
    //[DisplayName("获取触发器列表")]
    //public async Task<List<SysJobTrigger>> GetJobTriggerList([FromQuery] JobDetailInput input)
    //{
    //    return await _sysJobTriggerRep.AsQueryable()
    //        .WhereIF(!string.IsNullOrWhiteSpace(input.JobId), u => u.JobId.Contains(input.JobId))
    //        .ToListAsync();
    //}

    ///// <summary>
    ///// 添加触发器 ⏰
    ///// </summary>
    ///// <returns></returns>
    //[ApiDescriptionSettings(Name = "AddJobTrigger"), HttpPost]
    //[DisplayName("添加触发器")]
    //public async Task AddJobTrigger(AddJobTriggerInput input)
    //{
    //    var isExist = await _sysJobTriggerRep.IsAnyAsync(u => u.TriggerId == input.TriggerId && u.Id != input.Id);
    //    if (isExist)
    //        throw Oops.Oh(ErrorCodeEnum.D1006);

    //    var jobTrigger = input.Adapt<SysJobTrigger>();
    //    jobTrigger.Args = "[" + jobTrigger.Args + "]";

    //    var scheduler = _schedulerFactory.GetJob(input.JobId);
    //    scheduler?.AddTrigger(Triggers.Create(input.AssemblyName, input.TriggerType).LoadFrom(jobTrigger));
    //}

    ///// <summary>
    ///// 更新触发器 ⏰
    ///// </summary>
    ///// <returns></returns>
    //[ApiDescriptionSettings(Name = "UpdateJobTrigger"), HttpPost]
    //[DisplayName("更新触发器")]
    //public async Task UpdateJobTrigger(UpdateJobTriggerInput input)
    //{
    //    var isExist = await _sysJobTriggerRep.IsAnyAsync(u => u.TriggerId == input.TriggerId && u.Id != input.Id);
    //    if (isExist)
    //        throw Oops.Oh(ErrorCodeEnum.D1006);

    //    var jobTrigger = input.Adapt<SysJobTrigger>();
    //    jobTrigger.Args = "[" + jobTrigger.Args + "]";

    //    var scheduler = _schedulerFactory.GetJob(input.JobId);
    //    scheduler?.UpdateTrigger(Triggers.Create(input.AssemblyName, input.TriggerType).LoadFrom(jobTrigger));
    //}

    ///// <summary>
    ///// 删除触发器 ⏰
    ///// </summary>
    ///// <returns></returns>
    //[ApiDescriptionSettings(Name = "DeleteJobTrigger"), HttpPost]
    //[DisplayName("删除触发器")]
    //public async Task DeleteJobTrigger(DeleteJobTriggerInput input)
    //{
    //    var scheduler = _schedulerFactory.GetJob(input.JobId);
    //    scheduler?.RemoveTrigger(input.TriggerId);

    //    // 如果 _schedulerFactory 中不存在 JodId，则无法触发持久化，下行代码确保触发器能被删除
    //    await _sysJobTriggerRep.DeleteAsync(u => u.JobId == input.JobId && u.TriggerId == input.TriggerId);
    //}

    ///// <summary>
    ///// 暂停所有作业 ⏰
    ///// </summary>
    ///// <returns></returns>
    //[DisplayName("暂停所有作业")]
    //public void PauseAllJob()
    //{
    //    _schedulerFactory.PauseAll();
    //}

    ///// <summary>
    ///// 启动所有作业 ⏰
    ///// </summary>
    ///// <returns></returns>
    //[DisplayName("启动所有作业")]
    //public void StartAllJob()
    //{
    //    _schedulerFactory.StartAll();
    //}

    ///// <summary>
    ///// 暂停作业 ⏰
    ///// </summary>
    //[DisplayName("暂停作业")]
    //public void PauseJob(JobDetailInput input)
    //{
    //    _schedulerFactory.TryPauseJob(input.JobId, out _);
    //}

    ///// <summary>
    ///// 启动作业 ⏰
    ///// </summary>
    //[DisplayName("启动作业")]
    //public void StartJob(JobDetailInput input)
    //{
    //    _schedulerFactory.TryStartJob(input.JobId, out _);
    //}

    ///// <summary>
    ///// 取消作业 ⏰
    ///// </summary>
    //[DisplayName("取消作业")]
    //public void CancelJob(JobDetailInput input)
    //{
    //    _schedulerFactory.TryCancelJob(input.JobId, out _);
    //}

    ///// <summary>
    ///// 执行作业 ⏰
    ///// </summary>
    ///// <param name="input"></param>
    //[DisplayName("执行作业")]
    //public void RunJob(JobDetailInput input)
    //{
    //    if (_schedulerFactory.TryRunJob(input.JobId, out _) != ScheduleResult.Succeed)
    //        throw Oops.Oh(ErrorCodeEnum.D1705);
    //}

    ///// <summary>
    ///// 暂停触发器 ⏰
    ///// </summary>
    //[DisplayName("暂停触发器")]
    //public void PauseTrigger(JobTriggerInput input)
    //{
    //    var scheduler = _schedulerFactory.GetJob(input.JobId);
    //    scheduler?.PauseTrigger(input.TriggerId);
    //}

    ///// <summary>
    ///// 启动触发器 ⏰
    ///// </summary>
    //[DisplayName("启动触发器")]
    //public void StartTrigger(JobTriggerInput input)
    //{
    //    var scheduler = _schedulerFactory.GetJob(input.JobId);
    //    scheduler?.StartTrigger(input.TriggerId);
    //}

    ///// <summary>
    ///// 强制唤醒作业调度器 ⏰
    ///// </summary>
    //[DisplayName("强制唤醒作业调度器")]
    //public void CancelSleep()
    //{
    //    _schedulerFactory.CancelSleep();
    //}

    ///// <summary>
    ///// 强制触发所有作业持久化 ⏰
    ///// </summary>
    //[DisplayName("强制触发所有作业持久化")]
    //public void PersistAll()
    //{
    //    _schedulerFactory.PersistAll();
    //}

    ///// <summary>
    ///// 获取集群列表 ⏰
    ///// </summary>
    //[DisplayName("获取集群列表")]
    //public async Task<List<SysJobCluster>> GetJobClusterList()
    //{
    //    return await _sysJobClusterRep.GetListAsync();
    //}

    ///// <summary>
    ///// 获取作业触发器运行记录分页列表 ⏰
    ///// </summary>
    //[DisplayName("获取作业触发器运行记录分页列表")]
    //public async Task<SqlSugarPagedList<SysJobTriggerRecord>> PageJobTriggerRecord(PageJobTriggerRecordInput input)
    //{
    //    return await _sysJobTriggerRecordRep.AsQueryable()
    //        .WhereIF(!string.IsNullOrWhiteSpace(input.JobId), u => u.JobId.Contains(input.JobId))
    //        .WhereIF(!string.IsNullOrWhiteSpace(input.TriggerId), u => u.TriggerId.Contains(input.TriggerId))
    //        .OrderByDescending(u => u.Id)
    //        .ToPagedListAsync(input.Page, input.PageSize);
    //}
}