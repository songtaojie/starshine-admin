// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Elastic.Clients.Elasticsearch.Core.Search;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Admin.Models;
/// <summary>
/// 系统作业信息表
/// </summary>
//[SugarTable("QRTZ_JOB_DETAILS", "系统作业信息表")]
public class QrtzJobDetails:EntityBase
{
    /// <summary>
    /// 调度名字
    /// </summary>
    [SugarColumn(ColumnDescription = "调度名字", ColumnName = "SCHED_NAME",Length =120,IsNullable =false)]
    public string SchedulerName { get; set; }

    /// <summary>
    /// 任务名字
    /// </summary>
    [SugarColumn(ColumnDescription = "任务名字", ColumnName = "JOB_NAME", Length = 150, IsNullable = false)]
    public string JobName { get; set; }

    /// <summary>
    /// 任务分组
    /// </summary>
    [SugarColumn(ColumnDescription = "任务分组", ColumnName = "JOB_GROUP", Length = 150, IsNullable = false)]
    public string JobGroup { get; set; }

    /// <summary>
    /// 描述信息
    /// </summary>
    [SugarColumn(ColumnDescription = "描述信息", ColumnName = "Description",  Length = 128)]
    public string? Description { get; set; }

    /// <summary>
    /// 任务类名称
    /// </summary>
    [SugarColumn(ColumnDescription = "任务类名称", ColumnName = "JOB_CLASS_NAME", Length = 250)]
    public string? JobClassName { get; set; }

    /// <summary>
    /// 是否持久化
    /// </summary>
    [SugarColumn(ColumnDescription = "是否持久化", ColumnName = "IS_DURABLE")]
    public bool IsDurable { get; set; }

    /// <summary>
    /// 是否非并发
    /// </summary>
    [SugarColumn(ColumnDescription = "是否非并发", ColumnName = "IS_NONCONCURRENT")]
    public bool ISNonConcurrent{ get; set; }

    /// <summary>
    /// 是否更新数据
    /// 指示作业执行完成时是否应重新存储作业数据
    /// </summary>
    [SugarColumn(ColumnDescription = "是否更新数据", ColumnName = "IS_UPDATE_DATA")]
    public bool IsUpdateData { get; set; }

    /// <summary>
    /// 请求恢复
    /// 指导是否工作 如果出现“恢复”或“故障转移”情况，是否应该重新执行。
    /// </summary>
    [SugarColumn(ColumnDescription = "请求恢复", ColumnName = "REQUESTS_RECOVERY")]
    public bool RequestsRecovery { get; set; }

    /// <summary>
    /// 数据
    /// </summary>
    [SugarColumn(ColumnDescription = "数据", ColumnName = "JOB_DATA",ColumnDataType ="blob",IsNullable =true)]
    public byte[] JobData { get; set; }
}
