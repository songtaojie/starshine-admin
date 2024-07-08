﻿// MIT License
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
public abstract class TriggerAttribute : Attribute
{
    /// <summary>
    /// 私有开始时间
    /// </summary>
    private string? _startTime;

    /// <summary>
    /// 私有结束时间
    /// </summary>
    private string? _endTime;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="triggerType">作业触发器类型</param>
    /// <param name="args">作业触发器参数</param>
    public TriggerAttribute(TriggerTypeEnum triggerType, params object[] args)
    {
        TriggerType = triggerType;
        TriggerArgs = args;
        TriggerId = string.Empty;
    }

    /// <summary>
    /// 作业触发器 Id
    /// </summary>
    public string TriggerId { get; set; }

    /// <summary>
    /// 描述信息
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 起始时间
    /// </summary>
    public string? StartTime
    {
        get { return _startTime; }
        set
        {
            _startTime = value;

            // 解析运行时开始时间
            if (string.IsNullOrWhiteSpace(value)) RuntimeStartTime = null;
            else RuntimeStartTime = Convert.ToDateTime(value);
        }
    }

    /// <summary>
    /// 结束时间
    /// </summary>
    public string? EndTime
    {
        get { return _endTime; }
        set
        {
            _endTime = value;

            // 解析运行时结束时间
            if (string.IsNullOrWhiteSpace(value)) RuntimeEndTime = null;
            else RuntimeEndTime = Convert.ToDateTime(value);
        }
    }

    /// <summary>
    /// 最大触发次数
    /// </summary>
    /// <remarks>
    /// <para>0：不限制</para>
    /// <para>n：N 次</para>
    /// </remarks>
    public long MaxNumberOfRuns { get; set; }

    /// <summary>
    /// 最大出错次数
    /// </summary>
    /// <remarks>
    /// <para>0：不限制</para>
    /// <para>n：N 次</para>
    /// </remarks>
    public long MaxNumberOfErrors { get; set; }

    /// <summary>
    /// 重试次数
    /// </summary>
    public int NumRetries { get; set; } = 0;

    /// <summary>
    /// 重试间隔时间
    /// </summary>
    /// <remarks>默认1000毫秒</remarks>
    public int RetryTimeout { get; set; } = 1000;

    /// <summary>
    /// 是否立即启动
    /// </summary>
    public bool StartNow { get; set; } = true;

    /// <summary>
    /// 是否启动时执行一次
    /// </summary>
    public bool RunOnStart { get; set; } = false;

    /// <summary>
    /// 是否在启动时重置最大触发次数等于一次的作业
    /// </summary>
    /// <remarks>解决因持久化数据已完成一次触发但启动时不再执行的问题</remarks>
    public bool ResetOnlyOnce { get; set; } = true;

    /// <summary>
    /// 作业触发器运行时起始时间
    /// </summary>
    internal DateTime? RuntimeStartTime { get; set; }

    /// <summary>
    /// 作业触发器运行时结束时间
    /// </summary>
    internal DateTime? RuntimeEndTime { get; set; }

    /// <summary>
    /// 作业触发器运行时类型
    /// </summary>
    internal TriggerTypeEnum TriggerType { get; set; }

    /// <summary>
    /// 作业触发器运行时参数
    /// </summary>
    internal object[] TriggerArgs { get; set; }
}