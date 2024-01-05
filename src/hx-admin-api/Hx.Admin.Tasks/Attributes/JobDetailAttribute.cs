// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hx.Admin.Tasks;
/// <summary>
/// 配置作业信息特性
/// </summary>
/// <remarks>仅限 <see cref="Quartz.IJob"/> 实现类使用</remarks>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class JobDetailAttribute : Attribute
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="jobId">作业 Id</param>
    public JobDetailAttribute([NotNull]string jobId)
    {
        JobId = jobId;
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="jobId">作业 Id</param>
    /// <param name="description">作业描述</param>
    public JobDetailAttribute([NotNull] string jobId, string description)
        : this(jobId)
    {
        Description = description;
    }

    /// <summary>
    /// 作业 Id
    /// </summary>
    public string JobId { get; set; }

    /// <summary>
    /// 作业组名称
    /// </summary>
    public string? GroupName { get; set; }

    /// <summary>
    /// 描述信息
    /// </summary>
    public string? Description { get; set; }

}