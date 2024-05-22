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
/// Cron表达式的形式
/// </summary>
internal enum CronFormat
{
    /// <summary>
    /// 默认格式
    /// </summary>
    Default = 0,

    /// <summary>
    /// 每天特定小时开始作业格式
    /// </summary>
    DailyAt = 1
}
