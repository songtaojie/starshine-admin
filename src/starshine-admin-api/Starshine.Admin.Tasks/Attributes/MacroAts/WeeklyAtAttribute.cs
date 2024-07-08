// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Elastic.Clients.Elasticsearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Starshine.Admin.Tasks.Attributes.Cron;

/// <summary>
/// 每周特定星期几（午夜）开始作业触发器特性
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class WeeklyAtAttribute : CronTriggerAttribute
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="args">字段值</param>
    public WeeklyAtAttribute(params object[] args)
        : base(args)
    {
        CheckArgsNotNullOrEmpty(args);
        Cron = $"0 0 * * {FieldsToString(args)}";
    }
}