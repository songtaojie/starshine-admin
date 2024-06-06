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

namespace Hx.Admin.Models;

/// <summary>
/// 触发器执行记录
/// </summary>
[SugarTable("QRTZ_TRIGGERS_Record")]
[Tenant(SqlSugarConst.Quartz_ConfigId)]
public class QrtzTriggerRecord : EntityBase<int>
{
    /// <summary>
    /// 自增id
    /// </summary>
    [SugarColumn(ColumnDescription = "自增id", IsIdentity = true, IsPrimaryKey = true)]
    public override int Id { get; set; }
    /// <summary>
    /// 调度名字
    /// </summary>
    [SugarColumn(ColumnDescription = "调度名字", ColumnName = "SCHED_NAME", Length = 120, IsNullable = false)]
    public string SchedulerName { get; set; }

    /// <summary>
    /// 触发器名字
    /// </summary>
    [SugarColumn(ColumnDescription = "触发器名字", ColumnName = "TRIGGER_NAME", Length = 200, IsNullable = false)]
    public string TriggerName { get; set; }

    /// <summary>
    /// 触发器分组
    /// </summary>
    [SugarColumn(ColumnDescription = "触发器分组", ColumnName = "TRIGGER_GROUP", Length = 200, IsNullable = false)]
    public string TriggerGroup { get; set; }

    /// <summary>
    /// 任务名字
    /// </summary>
    [SugarColumn(ColumnDescription = "任务名字", ColumnName = "JOB_NAME", Length = 200, IsNullable = false)]
    public string JobName { get; set; }

    /// <summary>
    /// 任务分组
    /// </summary>
    [SugarColumn(ColumnDescription = "任务分组", ColumnName = "JOB_GROUP", Length = 200, IsNullable = false)]
    public string JobGroup { get; set; }

    /// <summary>
    /// 下次触发时间
    /// </summary>
    [SugarColumn(ColumnDescription = "下次触发时间", ColumnName = "NEXT_FIRE_TIME", IsNullable = true)]
    public long? NextFireTime { get; set; }

    /// <summary>
    /// 上次触发时间
    /// </summary>
    [SugarColumn(ColumnDescription = "上次触发时间", ColumnName = "PREV_FIRE_TIME", IsNullable = true)]
    public long? PrevFireTime { get; set; }

    /// <summary>
    /// 优先级
    /// </summary>
    [SugarColumn(ColumnDescription = "优先级", ColumnName = "PRIORITY")]
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
    [SugarColumn(ColumnDescription = "触发器状态", ColumnName = "TRIGGER_STATE", Length = 16, IsNullable = false)]
    public string TriggerState { get; set; }


    /// <summary>
    /// 触发器类型
    /// SIMPLE：简单触发器类型，
    /// CRON：Cron触发类型。
    /// CAL_INT：日历间隔触发类型。
    /// BLOB：一般的blob触发器类型。
    /// </summary>
    [SugarColumn(ColumnDescription = "触发器类型", ColumnName = "TRIGGER_TYPE", Length = 8, IsNullable = false)]
    public string TriggerType { get; set; }

    /// <summary>
    /// 本次执行耗时
    /// </summary>
    [SugarColumn(ColumnDescription = "本次执行耗时",Length =10,DecimalDigits =2)]
    public decimal ElapsedTime { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [SugarColumn(ColumnDescription = "创建时间")]
    public DateTime? CreatedTime { get; set; } = DateTime.Now;
}