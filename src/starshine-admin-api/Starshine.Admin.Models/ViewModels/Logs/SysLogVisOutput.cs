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

namespace Starshine.Admin.Models.ViewModels.Logs;
/// <summary>
/// 访问日志返回数据
/// </summary>
public class SysLogVisOutput
{
    /// <summary>
    /// 主键id
    /// </summary>
    public long Id { get; set; }
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
    /// 创建时间
    /// </summary>
    public DateTime? CreateTime { get; set; }

}
