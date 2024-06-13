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
/// <summary>
/// 集群列表返回数据
/// </summary>
public class PageSchedulerStateOutput
{
    /// <summary>
    /// 调度名字
    /// </summary>
    public string SchedulerName { get; set; }

    /// <summary>
    /// 实例名称
    /// </summary>
    public string InstanceName { get; set; }

    /// <summary>
    /// 上次检查时间
    /// </summary>
    public long LastCheckinTime { private get; set; }

    /// <summary>
    /// 上次检查时间
    /// </summary>
    public DateTime? LastCheckinTime_V => DateTimeUtil.GetDateTimeFromTicks(LastCheckinTime);

    /// <summary>
    /// 集群检查频率
    /// </summary>
    public long CheckinInterval { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public SchedulerStateEnum? State { get; set; }
}
