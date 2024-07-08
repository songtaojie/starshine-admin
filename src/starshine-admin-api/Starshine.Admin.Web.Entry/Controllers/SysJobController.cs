﻿// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Starshine.Admin.IServices;
using Starshine.Admin.Models.ViewModels.Job;
using Quartz;

namespace Starshine.Admin.Web.Entry.Controllers;

/// <summary>
/// 作业管理控制器
/// </summary>
public class SysJobController : AdminControllerBase
{ 
    private readonly ISysJobService _sysJobService;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sysJobService"></param>
    public SysJobController(ISysJobService sysJobService)
    {
        _sysJobService = sysJobService;
    }

    /// <summary>
    /// 获取作业分页列表 ⏰
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PagedListResult<PageJobDetailOutput>> GetJobDetailPage([FromQuery] PageJobDetailInput input)
    {
        return await _sysJobService.PageJobDetail(input);
    }

    /// <summary>
    /// 添加作业 ⏰
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task AddJobDetail(AddJobDetailInput input)
    {
        await _sysJobService.AddJobDetail(input);
    }

    /// <summary>
    /// 更新作业 ⏰
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task UpdateJobDetail(UpdateJobDetailInput input)
    {
        await _sysJobService.UpdateJobDetail(input);
    }

    /// <summary>
    /// 删除作业 ⏰
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task DeleteJobDetail(DeleteJobDetailInput input)
    {
        await _sysJobService.DeleteJobDetail(input);
    }

    /// <summary>
    /// 获取触发器列表 ⏰
    /// </summary>
    [HttpGet]
    public async Task<List<ListJobTriggerOutput>> GetJobTriggerList([FromQuery] ListJobTriggerInput input)
    { 
        return await _sysJobService.GetJobTriggerList(input);
    }

    /// <summary>
    /// 添加触发器 ⏰
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task AddJobTrigger(AddJobTriggerInput input)
    {
        await _sysJobService.AddJobTrigger(input);
    }

    /// <summary>
    /// 更新触发器 ⏰
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task UpdateJobTrigger(UpdateJobTriggerInput input)
    {
        await _sysJobService.UpdateJobTrigger(input);
    }

    /// <summary>
    /// 删除触发器 ⏰
    /// </summary>
    /// <returns></returns>
    [HttpDelete]
    public async Task DeleteJobTrigger(DeleteJobTriggerInput input)
    {
        await _sysJobService.DeleteJobTrigger(input);
    }

    /// <summary>
    /// 暂停所有作业 ⏰
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task PauseAllJob()
    {
        await _sysJobService.PauseAllJob();
    }

    /// <summary>
    /// 启动所有作业 ⏰
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task StartAllJob()
    {
        await _sysJobService.StartAllJob();
    }

    /// <summary>
    /// 暂停作业 ⏰
    /// </summary>
    [HttpPost]
    public async Task PauseJob(JobDetailInput input)
    {
        await _sysJobService.PauseJob(input);
    }

    /// <summary>
    /// 启动作业 ⏰
    /// </summary>
    [HttpPost]
    public async Task StartJob(JobDetailInput input)
    {
        await _sysJobService.StartJob(input);
    }

    /// <summary>
    /// 暂停触发器 ⏰
    /// </summary>
    [HttpPost]
    public async Task PauseTrigger(JobTriggerInput input)
    {
        await _sysJobService.PauseTrigger(input);
    }

    /// <summary>
    /// 启动触发器 ⏰
    /// </summary>
    [HttpPost]
    public async Task StartTrigger(JobTriggerInput input)
    {
        await _sysJobService.StartTrigger(input);
    }

    /// <summary>
    /// 获取作业触发器运行记录分页列表 ⏰
    /// </summary>
    [HttpGet]
    public async Task<PagedListResult<PageJobTriggerRecordOutput>> PageJobTriggerRecord([FromQuery]PageJobTriggerRecordInput input)
    {
        return await _sysJobService.PageJobTriggerRecord(input);
    }

    /// <summary>
    /// 获取集群列表 ⏰
    /// </summary>
    [HttpGet]
    public async Task<List<PageSchedulerStateOutput>> GetJobClusterList()
    {
        return await _sysJobService.GetJobClusterList();
    }
}
