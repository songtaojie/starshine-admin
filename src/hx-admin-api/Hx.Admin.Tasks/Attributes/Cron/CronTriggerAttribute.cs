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
/// 作业触发器特性基类
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class CronTriggerAttribute : TriggerAttribute
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="cron">cron表达式</param>
    public CronTriggerAttribute(string cron, params object[] args) : this(args)
    {
        Cron = cron;
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="triggerType">作业触发器类型</param>
    /// <param name="args">作业触发器参数</param>
    public CronTriggerAttribute(params object[] args) : base(TriggerTypeEnum.Corn, args)
    {
    }

    /// <summary>
    /// Cron表达式
    /// </summary>
    public string? Cron { get; protected set; }

    /// <summary>
    /// 检查参数非 Null 非空数组
    /// </summary>
    /// <param name="args">参数值</param>
    protected void CheckArgsNotNullOrEmpty(params object[] args)
    {
        // 空检查
        if (args == null || args.Length == 0) throw new ArgumentNullException(nameof(args));

        // 检查 fields 只能是 int, long，string 和非 null 类型
        if (args.Any(f => f == null || (f.GetType() != typeof(int) && f.GetType() != typeof(long) && f.GetType() != typeof(string)))) throw new InvalidOperationException("Invalid Cron expression.");
    }

    /// <summary>
    /// 将参数值转换成 string
    /// </summary>
    /// <param name="args">参数值</param>
    /// <returns><see cref="string"/></returns>
    protected string FieldsToString(params object[] args)
    {
        return string.Join(",", args);
    }
}