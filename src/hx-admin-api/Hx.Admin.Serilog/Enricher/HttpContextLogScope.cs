// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Serilog.Context;
using Serilog.Events;
using SqlSugar;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace Hx.Admin.Serilog.Enricher;
public class HttpContextLogScope : IDisposable
{

    private readonly Stack<IDisposable> _disposableStack = new Stack<IDisposable>();

    private readonly List<string> _loggerItems = new List<string>();

    public static HttpContextLogScope Instance => new();

    public void AddStock(IDisposable disposable)
    {
        _disposableStack.Push(disposable);
    }

    public void AddStock(string name,object? value)
    {
        AddStock(LogContext.PushProperty(name, value));
    }

    public IDisposable PushProperty(HttpContext httpContext)
    {

        var x_forwarded_for = new StringValues();
        if (httpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
        {
            x_forwarded_for = httpContext.Request.Headers["X-Forwarded-For"];
        }
        AddStock(LogContextConst.Client_Ip, JsonConvert.SerializeObject(x_forwarded_for));
        AddStock(LogContextConst.Request_Path, httpContext.Request.Path);
        AddStock(LogContextConst.Request_Method, httpContext.Request.Method);

        // 获取正在处理的路由数据
        var routeData = httpContext.GetRouteData();
        var controllerName = routeData.Values["controller"];
        var actionName = routeData.Values["action"];
        var areaName = routeData.DataTokens["area"];
        AddStock(LogContextConst.Route_Controller, controllerName?.ToString());
        AddStock(LogContextConst.Route_Action, actionName?.ToString());
        AddStock(LogContextConst.Route_Area, areaName?.ToString());
        var actionDescriptor = httpContext.GetControllerActionDescriptor();
        string? controllerTypeName = string.Empty;
        string? actionTypeName = string.Empty;
        string? displayName = string.Empty;
        if (actionDescriptor != null)
        {
            // 调用呈现链名称
            var actionMethod = actionDescriptor.MethodInfo!;
            controllerTypeName = actionMethod.DeclaringType?.Name;
            actionTypeName = actionMethod.Name;
            AddStock(LogContextConst.Route_ControllerType, controllerTypeName);
            AddStock(LogContextConst.Route_ActionType, actionMethod.Name);

            // 获取方法完整名称
            var methodFullName = actionMethod.DeclaringType?.FullName + "." + actionMethod.Name;
            AddStock(LogContextConst.Route_FullAction, methodFullName);
            // [DisplayName] 特性
            var displayNameAttribute = actionMethod.IsDefined(typeof(DisplayNameAttribute), true)
                ? actionMethod.GetCustomAttribute<DisplayNameAttribute>(true)
                : default;
            if (displayNameAttribute == null)
            {
                displayName = GetActionDescription(actionMethod);
            }
            else
            {
                displayName = displayNameAttribute.DisplayName;
            }
            AddStock(LogContextConst.Route_DisplayName, displayName);
            if (actionDescriptor.Parameters != null)
            {
                AddStock(LogContextConst.Route_ActionParameters, displayName);
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(LogContextConst.Route_ActionParameters, actionDescriptor.Parameters));
            }
        }

        // 获取 HttpContext 和 HttpRequest 对象
        var httpRequest = httpContext.Request;

        // 获取服务端 IPv4 地址
        var localIPv4 = httpContext.GetLocalIpAddressToIPv4();
        AddStock(LogContextConst.Request_LocalIPv4, localIPv4);

        // 获取服务端源端口
        var localPort = httpContext.Connection.LocalPort;
        AddStock(LogContextConst.Request_LocalPort, localPort);

        // 获取客户端 IPv4 地址
        var remoteIPv4 = httpContext.GetRemoteIpAddressToIPv4();
        AddStock(LogContextConst.Request_RemoteIPv4, remoteIPv4);

        // 获取客户端远程端口
        var remotePort = httpContext.Connection.RemotePort;
        AddStock(LogContextConst.Request_RemotePort, remotePort);

        // 客户端连接 ID
        var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;
        AddStock(LogContextConst.Request_TraceIdentifier, traceId);

        // 线程 Id
        var threadId = Environment.CurrentManagedThreadId;
        AddStock(LogContextConst.Request_TraceIdentifier, traceId);
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(LogContextConst.Request_ThreadId, threadId));

        // 获取请求的 Url 地址
        var requestUrl = Uri.UnescapeDataString(httpRequest.GetRequestUrlAddress());
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(LogContextConst.Request_FullUrl, requestUrl));

        // 获取来源 Url 地址
        var refererUrl = Uri.UnescapeDataString(httpRequest.GetRefererUrlAddress());
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(LogContextConst.Request_RefererUrl, refererUrl));

        // 客户端浏览器信息
        var userAgent = httpRequest.Headers["User-Agent"].ToString();
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(LogContextConst.Request_UserAgent, userAgent));

        // 客户端请求区域语言
        var acceptLanguage = httpRequest.Headers["accept-language"].ToString();
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(LogContextConst.Request_AcceptLanguage, acceptLanguage));

        // 请求来源（swagger还是其他）
        var requestFrom = httpRequest.Headers["request-from"].ToString();
        requestFrom = string.IsNullOrWhiteSpace(requestFrom) ? "client" : requestFrom;
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(LogContextConst.Request_RequestFrom, requestFrom));

        // 获取请求 cookies 信息
        var requestHeaderCookies = Uri.UnescapeDataString(httpRequest.Headers["cookie"].ToString());
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(LogContextConst.Request_HeaderCookies, requestHeaderCookies));

        var loggerItems = new List<string>()
                {
                    $"##控制器名称## {controllerTypeName}"
                    , $"##操作名称## {actionTypeName}"
                    , $"##显示名称## {displayName}"
                    , $"##路由信息## [area]: {areaName}; [controller]: {controllerName}; [action]: {actionName}"
                    , $"##请求方式## {httpContext.Request.Method}"
                    , $"##请求地址## {requestUrl}"
                    , $"##来源地址## {refererUrl}"
                    , $"##请求端源## {requestFrom}"
                    , $"##浏览器标识## {userAgent}"
                    , $"##客户端区域语言## {acceptLanguage}"
                    , $"##客户端 IP 地址## {remoteIPv4}"
                    , $"##服务端 IP 地址## {localIPv4}"
                    , $"##客户端连接 ID## {traceId}"
                    , $"##服务线程 ID## #{threadId}"
                };



















        AddStock(LogContext.PushProperty(LogContextStatic.LogSource, LogContextStatic.AopSql));
        AddStock(LogContext.PushProperty(LogContextStatic.SqlOutToConsole,
            AppSettings.app(new string[] { "AppSettings", "SqlAOP", "LogToConsole", "Enabled" }).ObjToBool()));
        AddStock(LogContext.PushProperty(LogContextStatic.SqlOutToFile,
            AppSettings.app(new string[] { "AppSettings", "SqlAOP", "LogToFile", "Enabled" }).ObjToBool()));
        AddStock(LogContext.PushProperty(LogContextStatic.OutToDb,
            AppSettings.app(new string[] { "AppSettings", "SqlAOP", "LogToDB", "Enabled" }).ObjToBool()));

        AddStock(LogContext.PushProperty(LogContextStatic.SugarActionType, db.SugarActionType));

        return this;
    }


    public void Dispose()
    {
        while (_disposableStack.Count > 0)
        {
            _disposableStack.Pop().Dispose();
        }
    }

    private static Dictionary<string, XPathNavigator> _xmlNavigatorDic = new Dictionary<string, XPathNavigator>();
    /// <summary>
    /// 获取action的信息
    /// </summary>
    /// <returns></returns>
    private string GetActionDescription(MethodInfo actionMethod)
    {
        var type = actionMethod.DeclaringType!;
        var xmlName = string.Format("{0}.xml", type.Assembly.GetName().Name);
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, xmlName);
        if (!System.IO.File.Exists(path)) return string.Empty;
        XPathNavigator? _xmlNavigator;
        if (!_xmlNavigatorDic.ContainsKey(path))
        {
            XPathDocument xmlDoc = new XPathDocument(path);
            _xmlNavigator = xmlDoc.CreateNavigator();
            _xmlNavigatorDic.Add(path, _xmlNavigator);
        }
        else
        {
            _xmlNavigator = _xmlNavigatorDic[path];
        }
        if (_xmlNavigator != null)
        {
            string memberXPath = "/doc/members/member[@name='{0}']";
            string summaryTag = "summary";
            var methodMemberName = XmlCommentsNodeNameHelper.GetMemberNameForMethod(actionMethod);
            var methodNode = _xmlNavigator.SelectSingleNode(string.Format(memberXPath, methodMemberName));
            if (methodNode != null)
            {
                var summaryNode = methodNode.SelectSingleNode(summaryTag);
                if (summaryNode != null)
                {
                    return XmlCommentsTextHelper.Humanize(summaryNode.InnerXml);
                }
            }
        }
        return string.Empty;
    }
}
