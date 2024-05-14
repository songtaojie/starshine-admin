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
/// 秒周期（间隔）作业触发器特性
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class PeriodSecondsAttribute : PeriodTriggerAttribute
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="interval">间隔（秒）</param>
    public PeriodSecondsAttribute(long interval)
        : base(interval * 1000)
    {
    }
}
