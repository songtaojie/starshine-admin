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

namespace Hx.Admin.Models.ViewModels.Job;
public class JobTriggerInput
{
    /// <summary>
    /// 触发器名字
    /// </summary>
    public string TriggerName { get; set; }

    /// <summary>
    /// 触发器分组
    /// </summary>
    public string TriggerGroup { get; set; }

    /// <summary>
    /// 任务名字
    /// </summary>
    public string JobName { get; set; }

    /// <summary>
    /// 任务分组
    /// </summary>
    public string JobGroup { get; set; }
}
