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

namespace Starshine.Admin.Models.ViewModels.Job;
public class PageJobTriggerRecordOutput
{
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
    /// 本次执行耗时
    /// </summary>
    public decimal ElapsedTime { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreatedTime { get; set; }
}
