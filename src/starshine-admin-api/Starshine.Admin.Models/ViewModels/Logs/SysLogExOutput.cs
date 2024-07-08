// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.Admin.Models.ViewModels.Logs;
/// <summary>
/// 异常日志
/// </summary>
public class SysLogExOutput
{
    /// <summary>
    /// 主键id
    /// </summary>
    public long Id {  get; set; }

    /// <summary>
    /// 模块名称
    /// </summary>
    public string? ControllerName { get; set; }

    /// <summary>
    /// 方法名称
    ///</summary>
    public string? ActionName { get; set; }

    /// <summary>
    /// 显示名称
    ///</summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// 执行状态
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// IP地址
    /// </summary>
    public string? RemoteIp { get; set; }

    /// <summary>
    /// 登录地点
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// 经度
    /// </summary>
    public double Longitude { get; set; }

    /// <summary>
    /// 维度
    /// </summary>
    public double Latitude { get; set; }

    /// <summary>
    /// 浏览器
    /// </summary>
    public string? Browser { get; set; }

    /// <summary>
    /// 操作系统
    /// </summary>
    public string? Os { get; set; }

    /// <summary>
    /// 操作用时
    /// </summary>
    public long Elapsed { get; set; }

    /// <summary>
    /// 账号
    /// </summary>
    public string? Account { get; set; }

    /// <summary>
    /// 真实姓名
    /// </summary>
    public string? RealName { get; set; }

    /// <summary>
    /// 请求方式
    /// </summary>
    public string? HttpMethod { get; set; }

    /// <summary>
    /// 请求地址
    /// </summary>
    public string? RequestUrl { get; set; }

    /// <summary>
    /// 请求参数
    /// </summary>
    public string? RequestParam { get; set; }

    /// <summary>
    /// 返回结果
    /// </summary>
    public string? ReturnResult { get; set; }

    /// <summary>
    /// 线程Id
    /// </summary>
    public int ThreadId { get; set; }

    /// <summary>
    /// 请求跟踪Id
    /// </summary>
    public string? TraceId { get; set; }

    /// <summary>
    /// 异常信息
    /// </summary>
    public string? Exception { get; set; }

    /// <summary>
    /// 日志消息Json
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// 日志级别
    /// </summary>
    public LogLevel? LogLevel { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreateTime { get; set; }
}
