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
/// 每小时特定分钟开始作业触发器特性
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class HourlyAtAttribute : CronTriggerAttribute
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="args">字段值</param>
    public HourlyAtAttribute(params object[] args)
        : base("@hourly", args)
    {
    }
}