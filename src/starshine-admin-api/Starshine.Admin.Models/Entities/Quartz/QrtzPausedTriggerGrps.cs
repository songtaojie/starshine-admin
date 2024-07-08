// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

namespace Starshine.Admin.Models;
/// <summary>
/// 暂停触发组
/// </summary>
[SugarTable("QRTZ_PAUSED_TRIGGER_GRPS", "系统Blob触发器")]
[Tenant(SqlSugarConst.Quartz_ConfigId)]
public class QrtzPausedTriggerGrps
{
    /// <summary>
    /// 调度名字
    /// </summary>
    [SugarColumn(ColumnDescription = "调度名字", ColumnName = "SCHED_NAME", IsNullable = false, IsPrimaryKey = true)]
    public string SchedulerName { get; set; }

    /// <summary>
    /// 触发器分组
    /// </summary>
    [SugarColumn(ColumnDescription = "触发器分组", ColumnName = "TRIGGER_GROUP", IsNullable = false, IsPrimaryKey = true)]
    public string TriggerGroup { get; set; }
}
