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
/// 触发器
/// </summary>
[SugarTable("QRTZ_FIRED_TRIGGERS")]
[Tenant(SqlSugarConst.Quartz_ConfigId)]
[SugarIndex("IDX_QRTZ_FT_TRIG_INST_NAME", nameof(SchedulerName),OrderByType.Asc,nameof(InstanceName),OrderByType.Asc)]
[SugarIndex("IDX_QRTZ_FT_INST_JOB_REQ_RCVRY", nameof(SchedulerName), OrderByType.Asc, nameof(InstanceName), OrderByType.Asc,nameof(RequestsRecovery),OrderByType.Asc)]
[SugarIndex("IDX_QRTZ_FT_J_G", nameof(SchedulerName), OrderByType.Asc, nameof(JobName), OrderByType.Asc, nameof(JobGroup), OrderByType.Asc)]
[SugarIndex("IDX_QRTZ_FT_JG", nameof(SchedulerName), OrderByType.Asc, nameof(JobGroup), OrderByType.Asc)]
[SugarIndex("IDX_QRTZ_FT_T_G", nameof(SchedulerName), OrderByType.Asc, nameof(TriggerName), OrderByType.Asc, nameof(TriggerGroup), OrderByType.Asc)]
[SugarIndex("IDX_QRTZ_FT_TG", nameof(SchedulerName), OrderByType.Asc, nameof(TriggerGroup), OrderByType.Asc)]
public class QrtzFiredTriggers
{
    /// <summary>
    /// 调度名字
    /// </summary>
    [SugarColumn(ColumnDescription = "调度名字", ColumnName = "SCHED_NAME", Length = 120, IsNullable = false, IsPrimaryKey = true)]
    public string SchedulerName { get; set; }

    /// <summary>
    /// 调度名字
    /// </summary>
    [SugarColumn(ColumnDescription = "调度名字", ColumnName = "ENTRY_ID", Length = 140, IsNullable = false, IsPrimaryKey = true)]
    public string EntryId { get; set; }

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
    /// 实例的名称
    /// </summary>
    [SugarColumn(ColumnDescription = "实例的名称", ColumnName = "INSTANCE_NAME", Length = 200, IsNullable = false)]
    public string InstanceName { get; set; }

    /// <summary>
    /// 触发时间
    /// </summary>
    [SugarColumn(ColumnDescription = "触发时间", ColumnName = "FIRED_TIME", Length =19, IsNullable = false)]
    public long FireTime { get; set; }

    /// <summary>
    /// 调度时间
    /// </summary>
    [SugarColumn(ColumnDescription = "调度时间", ColumnName = "SCHED_TIME", Length =19, IsNullable = false)]
    public long SchedTime { get; set; }

    /// <summary>
    /// 优先级
    /// </summary>
    [SugarColumn(ColumnDescription = "任务名字", ColumnName = "PRIORITY", Length =13, IsNullable = false)]
    public long Priority { get; set; }

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
    [SugarColumn(ColumnDescription = "状态", ColumnName = "STATE", Length = 16, IsNullable = false)]
    public string State { get; set; }


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
    /// 是否非并发
    /// </summary>
    [SugarColumn(ColumnDescription = "是否非并发", ColumnName = "IS_NONCONCURRENT", Length = 1)]
    public string ISNonConcurrent { get; set; }

    /// <summary>
    /// 请求恢复
    /// 指导是否工作 如果出现“恢复”或“故障转移”情况，是否应该重新执行。
    /// </summary>
    [SugarColumn(ColumnDescription = "请求恢复", ColumnName = "REQUESTS_RECOVERY",Length =1)]
    public string RequestsRecovery { get; set; }
}