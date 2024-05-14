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
[SugarTable(null, "系统作业信息表")]
public class QrtzJobDetails
{
    [SugarColumn(ColumnName = "SCHED_NAME",Length =120,IsNullable =false)]
    public string SchedName { get; set; }

    [SugarColumn(ColumnName = "JOB_NAME", Length = 150, IsNullable = false)]
    public string JobName { get; set; }

    [SugarColumn(ColumnName = "JOB_GROUP", Length = 150, IsNullable = false)]
    public string JobGroup { get; set; }

    /// <summary>
    /// 描述信息
    /// </summary>
    [SugarColumn(ColumnDescription = "描述信息", ColumnName = "DESCRIPTION",  Length = 128)]
    public string? Description { get; set; }

    /// <summary>
    /// 描述信息
    /// </summary>
    [SugarColumn(ColumnDescription = "描述信息", ColumnName = "JOB_CLASS_NAME", Length = 250)]
    public string? JobClassName { get; set; }

    /// <summary>
    /// 是否持久化
    /// </summary>
    [SugarColumn(ColumnDescription = "描述信息", ColumnName = "IS_DURABLE")]
    public bool IsDurable { get; set; }

    /// <summary>
    /// 是否非并发
    /// </summary>
    [SugarColumn(ColumnDescription = "描述信息", ColumnName = "IS_NONCONCURRENT")]
    public bool ISNonConcurrent{ get; set; }

    /// <summary>
    /// 是否非并发
    /// </summary>
    [SugarColumn(ColumnDescription = "描述信息", ColumnName = "IS_UPDATE_DATA")]
    public bool IsUpdateData { get; set; }

    /// <summary>
    /// 请求恢复
    /// </summary>
    [SugarColumn(ColumnDescription = "描述信息", ColumnName = "REQUESTS_RECOVERY")]
    public bool RequestsRecovery { get; set; }

    /// <summary>
    /// 数据
    /// </summary>
    [SugarColumn(ColumnDescription = "描述信息", ColumnName = "JOB_DATA",ColumnDataType ="blob")]
    public byte[] JobData { get; set; }
}
