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
public class ListJobTriggerInput
{
    /// <summary>
    /// 作业分组
    /// </summary>
    public string JobGroup { get; set; }
    /// <summary>
    /// 作业名字
    /// </summary>
    public string JobName { get; set; }
}
