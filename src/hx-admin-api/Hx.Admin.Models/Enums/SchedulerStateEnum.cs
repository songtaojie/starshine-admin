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

namespace Hx.Admin.Models;
public enum SchedulerStateEnum:uint
{
    /// <summary>
    /// 宕机
    /// </summary>
    Crashed,
    /// <summary>
    /// 工作中
    /// </summary>
    Working,
    /// <summary>
    /// 等待被唤醒
    /// </summary>
    Waiting
}
