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

namespace Hx.Admin.Tasks;
/// <summary>
/// 作业触发器特性基类
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class JobTriggerAttribute : Attribute
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="triggerId">触发器id</param>
    /// <param name="args">作业触发器参数</param>
    public JobTriggerAttribute(string triggerId)
    {
        TriggerId = triggerId;
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="triggerType">作业触发器类型</param>
    /// <param name="args">作业触发器参数</param>
    public JobTriggerAttribute(string triggerId, params object[] args) : this(triggerId)
    {
        RuntimeTriggerArgs = args;
    }

    /// <summary>
    /// Cron表达式
    /// </summary>
    public string? Cron { get; }

    /// <summary>
    /// 作业触发器 Id
    /// </summary>
    public string TriggerId { get; set; }

    /// <summary>
    /// 描述信息
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 是否立即启动
    /// </summary>
    public bool StartNow { get; set; } = true;

    /// <summary>
    /// 作业触发器运行时参数
    /// </summary>
    internal object[]? RuntimeTriggerArgs { get; set; }
}