﻿// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Admin.Models.ViewModels.Job;
/// <summary>
/// job分页列表
/// </summary>
public class PageJobDetailOutput
{
    /// <summary>
    /// 调度名字
    /// </summary>
    public string SchedulerName { get; set; }

    /// <summary>
    /// 任务名字
    /// </summary>
    public string JobName { get; set; }

    /// <summary>
    /// 任务分组
    /// </summary>
    public string JobGroup { get; set; }

    /// <summary>
    /// 描述信息
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 任务类名称
    /// </summary>
    public string? JobClassName { get; set; }

    /// <summary>
    /// 是否持久化
    /// </summary>
    public bool IsDurable { get; set; }

    /// <summary>
    /// 是否非并发
    /// </summary>
    public bool IsNonConcurrent { get; set; }

    /// <summary>
    /// 是否更新数据
    /// 指示作业执行完成时是否应重新存储作业数据
    /// </summary>
    public bool IsUpdateData { get; set; }

    /// <summary>
    /// 请求恢复
    /// 指导是否工作 如果出现“恢复”或“故障转移”情况，是否应该重新执行。
    /// </summary>
    public bool RequestsRecovery { get; set; }

    /// <summary>
    /// 作业创建类型
    /// </summary>
    public JobCreateTypeEnum CreateType { get; set; }

    /// <summary>
    /// 脚本代码
    /// </summary>
    public string? ScriptCode { get; set; }

    public IEnumerable<PageJobTriggersOutput> JobTriggers { get; set; }
}

public class PageJobTriggersOutput
{
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(IsIdentity = true)]
    public long Id { get; set; }
    /// <summary>
    /// 调度名字
    /// </summary>
    public string SchedulerName { get; set; }

    /// <summary>
    /// 触发器名字
    /// </summary>
    public string TriggerName { get; set; }

    /// <summary>
    /// 触发器分组
    /// </summary>
    public string TriggerGroup { get; set; }

    /// <summary>
    /// 任务名字
    /// </summary>
    public string JobName { get; set; }

    /// <summary>
    /// 任务分组
    /// </summary>
    public string JobGroup { get; set; }

    /// <summary>
    /// 描述信息
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 下次触发时间
    /// </summary>
    public long? NextFireTime { get; set; }

    /// <summary>
    /// 上次触发时间
    /// </summary>
    public long? PrevFireTime { get; set; }

    /// <summary>
    /// 优先级
    /// </summary>
    public int? Priority { get; set; }

    /// <summary>
    /// 触发器状态
    /// WAITING：等待，
    /// ACQUIRED，
    /// EXECUTING：执行中，
    /// COMPLETE：已完成，
    /// BLOCKED：阻塞，
    /// ERROR：错误，
    /// PAUSED：暂停，
    /// PAUSED_BLOCKED：暂停，
    /// DELETED：已删除
    /// </summary>
    public string TriggerState { get; set; }


    /// <summary>
    /// 触发器类型
    /// SIMPLE：简单触发器类型，
    /// CRON：Cron触发类型。
    /// CAL_INT：日历间隔触发类型。
    /// BLOB：一般的blob触发器类型。
    /// </summary>
    public string TriggerType { get; set; }

    /// <summary>
    /// 触发开始时间
    /// </summary>
    public long StartTime { get; set; }

    /// <summary>
    /// 触发截止时间
    /// </summary>
    public long? EndTime { get; set; }

    /// <summary>
    /// 日历名字
    /// </summary>
    public string? CalenderName { get; set; }

    /// <summary>
    /// 失败的指令。
    /// </summary>
    public int? MisfireInstructions { get; set; }

    /// <summary>
    /// 数据
    /// </summary>
    public byte[] JobData { get; set; }
}