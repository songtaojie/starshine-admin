// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

namespace Hx.Admin.Models;
/// <summary>
/// 触发器
/// </summary>
[SugarTable("QRTZ_CRON_TRIGGERS", "系统Cron表达式触发器")]
[Tenant(SqlSugarConst.Quartz_ConfigId)]
public class QrtzCronTriggers
{
    /// <summary>
    /// 调度名字
    /// </summary>
    [SugarColumn(ColumnDescription = "调度名字", ColumnName = "SCHED_NAME", Length = 120, IsNullable = false, IsPrimaryKey = true)]
    public string SchedulerName { get; set; }

    /// <summary>
    /// 触发器名字
    /// </summary>
    [SugarColumn(ColumnDescription = "触发器名字", ColumnName = "TRIGGER_NAME", Length = 150, IsNullable = false, IsPrimaryKey = true)]
    public string TriggerName { get; set; }

    /// <summary>
    /// 触发器分组
    /// </summary>
    [SugarColumn(ColumnDescription = "触发器分组", ColumnName = "TRIGGER_GROUP", Length = 150, IsNullable = false, IsPrimaryKey = true)]
    public string TriggerGroup { get; set; }

    /// <summary>
    ///Cron表达式
    /// </summary>
    [SugarColumn(ColumnDescription = "Cron表达式", ColumnName = "CRON_EXPRESSION", Length = 250, IsNullable = false)]
    public string CronExpression { get; set; }

    /// <summary>
    ///时区id
    /// </summary>
    [SugarColumn(ColumnDescription = "时区id", ColumnName = "TIME_ZONE_ID",Length = 80)]
    public string? TimeZoneId { get; set; }
}