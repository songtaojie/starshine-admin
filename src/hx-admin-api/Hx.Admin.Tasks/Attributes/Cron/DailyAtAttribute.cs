// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hx.Admin.Tasks;
/// <summary>
/// 每天特定小时开始作业触发器特性
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class DailyAtAttribute : CronTriggerAttribute
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="args">参数值</param>
    public DailyAtAttribute(params object[] args)
        : base("@daily", args)
    {
    }
}
