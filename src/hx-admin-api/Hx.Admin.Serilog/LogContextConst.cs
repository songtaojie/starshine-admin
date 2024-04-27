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

namespace Hx.Admin.Serilog;
public class LogContextConst
{
    /// <summary>
    /// 用户id
    /// </summary>
    public const string Request_UserId = "Request_UserId";

    /// <summary>
    /// 请求的Claims
    /// </summary>
    public const string Request_Claims = "Request_Claims";

    /// <summary>
    /// 请求时间
    /// </summary>
    public const string Request_ElapsedMilliseconds = "RequestElapsedMilliseconds";

    /// <summary>
    /// 客户端ip
    /// </summary>
    public const string Client_Ip = "Client_Ip";

    /// <summary>
    /// 请求路径
    /// </summary>
    public const string Request_Path = "Request_Path";

    /// <summary>
    /// HTTP请求方法
    /// </summary>
    public const string Request_Method = "Request_Method";

    /// <summary>
    /// 请求的控制器
    /// </summary>
    public const string Route_Controller = "Route_Controller";

    /// <summary>
    /// 请求的action
    /// </summary>
    public const string Route_Action = "Route_Action";

    /// <summary>
    /// 请求的area
    /// </summary>
    public const string Route_Area = "Route_Area";

    /// <summary>
    /// 请求的控制器类型
    /// </summary>
    public const string Route_ControllerType = "Route_ControllerType";
    
    /// <summary>
    /// 请求的action类型
    /// </summary>
    public const string Route_ActionType = "Route_ActionType";

    /// <summary>
    /// 请求的action类型
    /// </summary>
    public const string Route_ActionParameters = "Route_ActionParameters";

    /// <summary>
    /// 请求的action结果
    /// </summary>
    public const string Route_ActionResult = "Route_ActionResult";

    /// <summary>
    /// 请求的action类型
    /// </summary>
    public const string Route_FullAction = "Route_FullAction";

    /// <summary>
    /// HTTP响应代码
    /// </summary>
    public const string Route_DisplayName = "Route_DisplayName";

    /// <summary>
    /// 本机IpV4
    /// </summary>
    public const string Request_LocalIPv4 = "Request_LocalIPv4";

    /// <summary>
    /// 本地端口
    /// </summary>
    public const string Request_LocalPort = "Request_LocalPort";

    /// <summary>
    /// 远程IpV4
    /// </summary>
    public const string Request_RemoteIPv4 = "Request_RemoteIPv4";

    /// <summary>
    /// 远程端口
    /// </summary>
    public const string Request_RemotePort = "Request_LocalPort";

    /// <summary>
    /// 客户端连接 ID
    /// </summary>
    public const string Request_TraceIdentifier = "Request_TraceIdentifier";

    /// <summary>
    /// 线程 Id
    /// </summary>
    public const string Request_ThreadId = "Request_ThreadId";

    /// <summary>
    /// 完整请求地址
    /// </summary>
    public const string Request_FullUrl = "Request_FullUrl";

    /// <summary>
    /// 来源 Url 地址
    /// </summary>
    public const string Request_RefererUrl = "Request_RefererUrl";

    /// <summary>
    /// 客户端浏览器信息
    /// </summary>
    public const string Request_UserAgent = "Request_UserAgent";

    /// <summary>
    /// 客户端请求区域语言
    /// </summary>
    public const string Request_AcceptLanguage = "Request_AcceptLanguage";

    /// <summary>
    /// 请求来源（swagger还是其他）
    /// </summary>
    public const string Request_RequestFrom = "Request_RequestFrom";

    /// <summary>
    /// 请求 cookies 信息
    /// </summary>
    public const string Request_HeaderCookies = "Request_HeaderCookies";

    /// <summary>
    /// HTTP响应代码
    /// </summary>
    public const string Response_StatusCode = "Response_StatusCode";
}
