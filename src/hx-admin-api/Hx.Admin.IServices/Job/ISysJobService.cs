// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Admin.IService;
using Hx.Admin.Models;
using Hx.Admin.Models.ViewModels.Job;
using Hx.Common.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Admin.IServices;
public interface ISysJobService : IBaseService<QrtzJobDetails,int>, IScopedDependency
{
    /// <summary>
    /// 获取作业分页列表 ⏰
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PagedListResult<PageJobDetailOutput>> PageJobDetail(PageJobDetailInput input);

    /// <summary>
    /// 添加触发记录
    /// </summary>
    /// <param name="addTriggerRecordInput">触发记录</param>
    Task AddTriggerRecord(AddTriggerRecordInput addTriggerRecordInput);

    /// <summary>
    /// 初始化DbJob
    /// </summary>
    /// <param name="quartzOptions">动态编译的作业代码</param>
    Task ScanDbJobToQuartz(QuartzOptions quartzOptions);

    /// <summary>
    /// 添加作业 ⏰
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task AddJobDetail(AddJobDetailInput input);

    /// <summary>
    /// 更新作业 ⏰
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task UpdateJobDetail(UpdateJobDetailInput input);

    /// <summary>
    /// 删除作业 ⏰
    /// </summary>
    /// <returns></returns>
    Task DeleteJobDetail(DeleteJobDetailInput input);

    /// <summary>
    /// 获取触发器列表 ⏰
    /// </summary>
    Task<List<ListJobTriggerOutput>> GetJobTriggerList(ListJobTriggerInput input);

    /// <summary>
    /// 添加触发器 ⏰
    /// </summary>
    /// <returns></returns>
    Task AddJobTrigger(AddJobTriggerInput input);

    /// <summary>
    /// 更新触发器 ⏰
    /// </summary>
    /// <returns></returns>
    Task UpdateJobTrigger(UpdateJobTriggerInput input);

    /// <summary>
    /// 删除触发器 ⏰
    /// </summary>
    /// <returns></returns>
    Task DeleteJobTrigger(DeleteJobTriggerInput input);

    /// <summary>
    /// 暂停所有作业 ⏰
    /// </summary>
    /// <returns></returns>
    Task PauseAllJob();

    /// <summary>
    /// 启动所有作业 ⏰
    /// </summary>
    /// <returns></returns>
    Task StartAllJob();

    /// <summary>
    /// 暂停作业 ⏰
    /// </summary>
    Task PauseJob(JobDetailInput input);

    /// <summary>
    /// 启动作业 ⏰
    /// </summary>
    Task StartJob(JobDetailInput input);

    /// <summary>
    /// 暂停触发器 ⏰
    /// </summary>
    Task PauseTrigger(JobTriggerInput input);

    /// <summary>
    /// 启动触发器 ⏰
    /// </summary>
    Task StartTrigger(JobTriggerInput input);

    /// <summary>
    /// 获取作业触发器运行记录分页列表 ⏰
    /// </summary>
    Task<PagedListResult<PageJobTriggerRecordOutput>> PageJobTriggerRecord(PageJobTriggerRecordInput input);
}
