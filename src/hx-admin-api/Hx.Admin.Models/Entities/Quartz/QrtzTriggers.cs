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
[SugarTable("QRTZ_TRIGGERS")]
[Tenant(SqlSugarConst.Quartz_ConfigId)]
[SugarIndex("IDX_QRTZ_T_J", nameof(SchedulerName), OrderByType.Asc, nameof(JobGroup), OrderByType.Desc, nameof(JobName), OrderByType.Desc)]
[SugarIndex("IDX_QRTZ_T_JG", nameof(SchedulerName), OrderByType.Asc, nameof(JobGroup), OrderByType.Desc)]
[SugarIndex("IDX_QRTZ_T_C", nameof(SchedulerName), OrderByType.Asc, nameof(CalenderName), OrderByType.Desc, nameof(JobName), OrderByType.Desc)]
[SugarIndex("IDX_QRTZ_T_G", nameof(SchedulerName), OrderByType.Asc, nameof(TriggerGroup), OrderByType.Desc)]
[SugarIndex("IDX_QRTZ_T_STATE", nameof(SchedulerName), OrderByType.Asc, nameof(TriggerState), OrderByType.Desc)]
[SugarIndex("IDX_QRTZ_T_N_STATE", nameof(SchedulerName), OrderByType.Asc, nameof(TriggerName), OrderByType.Desc, nameof(TriggerGroup), OrderByType.Desc, nameof(TriggerState), OrderByType.Desc)]
[SugarIndex("IDX_QRTZ_T_N_G_STATE", nameof(SchedulerName), OrderByType.Asc, nameof(TriggerGroup), OrderByType.Desc, nameof(TriggerState), OrderByType.Desc)]
[SugarIndex("IDX_QRTZ_T_NEXT_FIRE_TIME", nameof(SchedulerName), OrderByType.Asc, nameof(NextFireTime), OrderByType.Desc)]
[SugarIndex("IDX_QRTZ_T_NFT_ST", nameof(SchedulerName), OrderByType.Asc, nameof(TriggerState), OrderByType.Desc, nameof(NextFireTime), OrderByType.Desc)]
[SugarIndex("IDX_QRTZ_T_NFT_MISFIRE", nameof(SchedulerName), OrderByType.Asc, nameof(MisfireInstructions), OrderByType.Desc, nameof(NextFireTime), OrderByType.Desc)]
[SugarIndex("IDX_QRTZ_T_NFT_ST_MISFIRE", nameof(SchedulerName), OrderByType.Asc, nameof(MisfireInstructions), OrderByType.Desc, nameof(NextFireTime), OrderByType.Desc, nameof(TriggerState), OrderByType.Desc)]
[SugarIndex("IDX_QRTZ_T_NFT_ST_MISFIRE_GRP", nameof(SchedulerName), OrderByType.Asc, nameof(MisfireInstructions), OrderByType.Desc, nameof(NextFireTime), OrderByType.Desc, nameof(TriggerGroup), OrderByType.Desc, nameof(TriggerState), OrderByType.Desc)]
public class QrtzTriggers:EntityBase<int>
{
    /// <summary>
    /// 自增id
    /// </summary>
    [SugarColumn(ColumnDescription = "自增id", IsIdentity = true, IsPrimaryKey = true)]
    public override int Id { get; set; }
    /// <summary>
    /// 调度名字
    /// </summary>
    [SugarColumn(ColumnDescription = "调度名字", ColumnName = "SCHED_NAME", Length =120,  IsNullable = false)]
    public string SchedulerName { get; set; }

    /// <summary>
    /// 触发器名字
    /// </summary>
    [SugarColumn(ColumnDescription = "触发器名字", ColumnName = "TRIGGER_NAME", Length =200, IsNullable = false)]
    public string TriggerName { get; set; }

    /// <summary>
    /// 触发器分组
    /// </summary>
    [SugarColumn(ColumnDescription = "触发器分组", ColumnName = "TRIGGER_GROUP",Length =200, IsNullable = false)]
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
    /// 描述信息
    /// </summary>
    [SugarColumn(ColumnDescription = "描述信息", ColumnName = "Description", Length =250, IsNullable = true)]
    public string? Description { get; set; }

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
    [SugarColumn(ColumnDescription = "触发器状态", ColumnName = "TRIGGER_STATE", Length =16, IsNullable =false)]
    public string TriggerState { get; set; }


    /// <summary>
    /// 触发器类型
    /// SIMPLE：简单触发器类型，
    /// CRON：Cron触发类型。
    /// CAL_INT：日历间隔触发类型。
    /// BLOB：一般的blob触发器类型。
    /// </summary>
    [SugarColumn(ColumnDescription = "触发器类型", ColumnName = "TRIGGER_TYPE", Length = 8,IsNullable = false)]
    public string TriggerType { get; set; }

    /// <summary>
    /// 触发开始时间
    /// </summary>
    [SugarColumn(ColumnDescription = "触发开始时间", ColumnName = "START_TIME", IsNullable = false)]
    public long StartTime { get; set; }

    /// <summary>
    /// 触发截止时间
    /// </summary>
    [SugarColumn(ColumnDescription = "触发截止时间", ColumnName = "END_TIME",  IsNullable = true)]
    public long? EndTime { get; set; }

    /// <summary>
    /// 日历名字
    /// </summary>
    [SugarColumn(ColumnDescription = "日历名字", ColumnName = "CALENDAR_NAME", Length = 200, IsNullable = true)]
    public string? CalenderName { get; set; }

    /// <summary>
    /// 失败的指令。
    /// </summary>
    [SugarColumn(ColumnDescription = "失败的指令。", ColumnName = "MISFIRE_INSTR", IsNullable = true)]
    public int? MisfireInstructions { get; set; }

    /// <summary>
    /// 数据
    /// </summary>
    [SugarColumn(ColumnDescription = "数据", ColumnName = "JOB_DATA", ColumnDataType = "BLOB", IsNullable =true)]
    public byte[] JobData { get; set; }
}
