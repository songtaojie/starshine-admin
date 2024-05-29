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
/// 添加作业 input
/// </summary>
public class AddJobDetailInput
{
    /// <summary>
    /// 调度名字
    /// </summary>
    public string SchedulerName { get; set; }

    /// <summary>
    /// 任务名字
    /// </summary>
    public string JobName { get; set; }

    /// <summary>
    /// 任务分组
    /// </summary>
    public string JobGroup { get; set; }

    /// <summary>
    /// 描述信息
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 任务类名称
    /// </summary>
    public string? JobClassName { get; set; }

    /// <summary>
    /// 是否持久化
    /// </summary>
    public bool IsDurable { get; set; }

    /// <summary>
    /// 是否非并发
    /// </summary>
    public bool IsNonConcurrent { get; set; }

    /// <summary>
    /// 是否更新数据
    /// 指示作业执行完成时是否应重新存储作业数据
    /// </summary>
    public bool IsUpdateData { get; set; }

    /// <summary>
    /// 请求恢复
    /// 指导是否工作 如果出现“恢复”或“故障转移”情况，是否应该重新执行。
    /// </summary>
    public bool RequestsRecovery { get; set; }

    /// <summary>
    /// 数据
    /// </summary>
    public byte[] JobData { get; set; }

    /// <summary>
    /// 作业创建类型
    /// </summary>
    public JobCreateTypeEnum CreateType { get; set; }

    /// <summary>
    /// 脚本代码
    /// </summary>
    public string? ScriptCode { get; set; }
}
