// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Admin.Models;
/// <summary>
/// 触发器
/// </summary>
//[SugarTable("QRTZ_SIMPLE_TRIGGERS")]
public class QrtzSimpleTriggers:EntityBase
{
    /// <summary>
    /// 调度名字
    /// </summary>
    [SugarColumn(ColumnDescription = "调度名字", ColumnName = "SCHED_NAME", Length = 120, IsNullable = false)]
    public string SchedulerName { get; set; }

    /// <summary>
    /// 触发器名字
    /// </summary>
    [SugarColumn(ColumnDescription = "触发器名字", ColumnName = "TRIGGER_NAME", Length = 150, IsNullable = false)]
    public string TriggerName { get; set; }

    /// <summary>
    /// 触发器分组
    /// </summary>
    [SugarColumn(ColumnDescription = "触发器分组", ColumnName = "TRIGGER_GROUP", Length = 150, IsNullable = false)]
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
