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

namespace Starshine.Admin.Models.ViewModels.Logs;
/// <summary>
/// 差异化日志返回结果
/// </summary>
public class SysLogDiffOutput
{
    /// <summary>
    /// 主键id
    /// </summary>
    public long Id {  get; set; }

    /// <summary>
    /// 操作前记录
    /// </summary>
    public string? BeforeData { get; set; }

    /// <summary>
    /// 操作后记录
    /// </summary>
    public string? AfterData { get; set; }

    /// <summary>
    /// Sql
    /// </summary>
    public string? Sql { get; set; }

    /// <summary>
    /// 参数  手动传入的参数
    /// </summary>
    public string? Parameters { get; set; }

    /// <summary>
    /// 业务对象
    /// </summary>
    public string? BusinessData { get; set; }

    /// <summary>
    /// 差异操作
    /// </summary>
    public string? DiffType { get; set; }

    /// <summary>
    /// 耗时
    /// </summary>
    public long Elapsed { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreateTime { get; set; }
}
