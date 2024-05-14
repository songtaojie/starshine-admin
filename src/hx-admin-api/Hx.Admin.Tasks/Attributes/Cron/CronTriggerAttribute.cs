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
public class CronTriggerAttribute : TriggerAttribute
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="triggerId">triggerId</param>
    public CronTriggerAttribute(string triggerId, params object[] args) : this(args)
    {
        TriggerId = triggerId;
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="triggerType">作业触发器类型</param>
    /// <param name="args">作业触发器参数</param>
    public CronTriggerAttribute(params object[] args) : base(TriggerTypeEnum.Corn, args)
    {
    }

    /// <summary>
    /// Cron表达式
    /// </summary>
    public string? Cron { get; set; }
}