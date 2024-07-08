﻿// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Starshine.Common;

namespace Starshine.Admin.Models;
/// <summary>
/// 触发器
/// </summary>
[SugarTable("QRTZ_SIMPLE_TRIGGERS", "系统简单触发器")]
[Tenant(SqlSugarConst.Quartz_ConfigId)]
public class QrtzSimpleTriggers:EntityBase<int>
{
    /// <summary>
    /// 自增id
    /// </summary>
    [SugarColumn(ColumnDescription = "自增id", IsIdentity = true, IsPrimaryKey = true)]
    public override int Id { get; set; }

    /// <summary>
    /// 调度名字
    /// </summary>
    [SugarColumn(ColumnDescription = "调度名字", ColumnName = "SCHED_NAME", Length =120, IsNullable = false)]
    public string SchedulerName { get; set; }

    /// <summary>
    /// 触发器名字
    /// </summary>
    [SugarColumn(ColumnDescription = "触发器名字", ColumnName = "TRIGGER_NAME", Length =200, IsNullable = false)]
    public string TriggerName { get; set; }

    /// <summary>
    /// 触发器分组
    /// </summary>
    [SugarColumn(ColumnDescription = "触发器分组", ColumnName = "TRIGGER_GROUP", Length =200, IsNullable = false)]
    public string TriggerGroup { get; set; }

    /// <summary>
    ///重复次数
    /// </summary>
    [SugarColumn(ColumnDescription = "重复次数", ColumnName = "REPEAT_COUNT", IsNullable = false)]
    public long RepeatCount { get; set; }

    /// <summary>
    ///重复间隔
    /// </summary>
    [SugarColumn(ColumnDescription = "重复间隔", ColumnName = "REPEAT_INTERVAL", IsNullable = false)]
    public long RepeatInterval { get; set; }

    /// <summary>
    ///触发次数
    /// </summary>
    [SugarColumn(ColumnDescription = "触发次数", ColumnName = "TIMES_TRIGGERED", IsNullable = false)]
    public long TriggeredTimes { get; set; }
}
