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

namespace Starshine.Admin.Tasks;

/// <summary>
/// 作业触发器特性基类
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class PeriodTriggerAttribute : TriggerAttribute
{
    protected PeriodTriggerAttribute(long interval) : base(TriggerTypeEnum.Simple, interval)
    {
    }
}
