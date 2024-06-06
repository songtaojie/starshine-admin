// MIT License
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
public class AddJobTriggerInput
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
