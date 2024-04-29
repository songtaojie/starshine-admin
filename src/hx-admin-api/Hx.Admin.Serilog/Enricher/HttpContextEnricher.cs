// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Common.Extensions;
using Hx.Sdk.Core;
using Magicodes.ExporterAndImporter.Excel.Utility.TemplateExport;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Qiniu.Storage;
using Qiniu.Util;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hx.Admin.Serilog.Enricher;
public class HttpContextEnricher : ILogEventEnricher
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Action<LogEvent, ILogEventPropertyFactory, HttpContext> _enrichAction;
    

    public HttpContextEnricher(IServiceProvider serviceProvider) : this(serviceProvider, null)
    { }

    public HttpContextEnricher(IServiceProvider serviceProvider, Action<LogEvent, ILogEventPropertyFactory, HttpContext>? enrichAction)
    {
        _serviceProvider = serviceProvider;
        if (enrichAction == null)
        {
            _enrichAction = (logEvent, propertyFactory, httpContext) =>
            {
                var x_forwarded_for = new StringValues();
                if (httpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
                {
                    x_forwarded_for = httpContext.Request.Headers["X-Forwarded-For"];
                }
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(LogContextConst.Client_Ip, JsonConvert.SerializeObject(x_forwarded_for)));
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(LogContextConst.Request_Path, httpContext.Request.Path));
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(LogContextConst.Request_Method, httpContext.Request.Method));
                if (httpContext.Response.HasStarted)
                {
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(LogContextConst.Response_StatusCode, httpContext.Response.StatusCode));
                }
                var actionDescriptor = httpContext.RequestServices.GetService<ControllerActionDescriptor>();
                // 获取正在处理的路由数据
                var routeData = httpContext.GetRouteData();
                var controllerName = routeData.Values["controller"];
                var actionName = routeData.Values["action"];
                var areaName = routeData.DataTokens["area"];
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(LogContextConst.Route_Controller, controllerName?.ToString()));
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(LogContextConst.Route_Action, actionName?.ToString()));
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(LogContextConst.Route_Area, areaName?.ToString()));
                actionDescriptor ??= httpContext.GetControllerActionDescriptor();
                string? controllerTypeName = string.Empty;
                string? actionTypeName = string.Empty;
                string? displayName = string.Empty;
                if (actionDescriptor != null)
                {
                    // 调用呈现链名称
                    var actionMethod = actionDescriptor.MethodInfo!;
                    controllerTypeName = actionMethod.DeclaringType?.Name;
                    actionTypeName = actionMethod.Name;
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(LogContextConst.Route_ControllerType, controllerTypeName));
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(LogContextConst.Route_ActionType, actionMethod.Name));
                   
                    // 获取方法完整名称
                    var methodFullName = actionMethod.DeclaringType?.FullName + "." + actionMethod.Name;
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(LogContextConst.Route_FullAction, methodFullName));
                    // [DisplayName] 特性
                    var displayNameAttribute = actionMethod.IsDefined(typeof(DisplayNameAttribute), true)
                        ? actionMethod.GetCustomAttribute<DisplayNameAttribute>(true)
                        : default;
                    displayName = displayNameAttribute?.DisplayName;
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(LogContextConst.Route_DisplayName, displayNameAttribute?.DisplayName));
                    if (actionDescriptor.Parameters != null)
                    {
                        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(LogContextConst.Route_ActionParameters, actionDescriptor.Parameters));
                        
                    }
                }

                // 获取 HttpContext 和 HttpRequest 对象
                var httpRequest = httpContext.Request;

                // 获取服务端 IPv4 地址
                var localIPv4 = httpContext.GetLocalIpAddressToIPv4();
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(LogContextConst.Request_LocalIPv4, localIPv4));

                // 获取服务端源端口
                var localPort = httpContext.Connection.LocalPort;
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(LogContextConst.Request_LocalPort, localPort));

                // 获取客户端 IPv4 地址
                var remoteIPv4 = httpContext.GetRemoteIpAddressToIPv4();
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(LogContextConst.Request_RemoteIPv4, remoteIPv4));

                // 获取客户端远程端口
                var remotePort = httpContext.Connection.RemotePort;
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(LogContextConst.Request_RemotePort, remotePort));

                // 客户端连接 ID
                var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier ;
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(LogContextConst.Request_TraceIdentifier, traceId));

                // 线程 Id
                var threadId = Environment.CurrentManagedThreadId;
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(LogContextConst.Request_ThreadId, threadId));

                // 获取请求的 Url 地址
                var requestUrl = Uri.UnescapeDataString(httpRequest.GetRequestUrlAddress());
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(LogContextConst.Request_FullUrl, requestUrl));

                // 获取来源 Url 地址
                var refererUrl = Uri.UnescapeDataString(httpRequest.GetRefererUrlAddress());
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(LogContextConst.Request_RefererUrl, refererUrl));

                // 客户端浏览器信息
                var userAgent = httpRequest.Headers["User-Agent"];
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(LogContextConst.Request_UserAgent, userAgent));

                // 客户端请求区域语言
                var acceptLanguage = httpRequest.Headers["accept-language"];
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(LogContextConst.Request_AcceptLanguage, acceptLanguage));

                // 请求来源（swagger还是其他）
                var requestFrom = httpRequest.Headers["request-from"].ToString();
                requestFrom = string.IsNullOrWhiteSpace(requestFrom) ? "client" : requestFrom;
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(LogContextConst.Request_RequestFrom, requestFrom));

               
                // 获取请求 cookies 信息
                var requestHeaderCookies = Uri.UnescapeDataString(httpRequest.Headers["cookie"].ToString());
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(LogContextConst.Request_HeaderCookies, requestHeaderCookies));
                
                //获取授权信息
                if (httpContext.User != null && httpContext.User.Identity != null && httpContext.User.Identity.IsAuthenticated)
                {
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(LogContextConst.Request_Claims, httpContext.User.Claims));
                }
                // 获取响应 cookies 信息
                var responseHeaderCookies = Uri.UnescapeDataString(httpContext.Response.Headers["Set-Cookie"].ToString());
                var osDescription = RuntimeInformation.OSDescription;
                var osArchitecture = RuntimeInformation.OSArchitecture.ToString();
                var frameworkDescription = RuntimeInformation.FrameworkDescription;
                var basicFrameworkDescription = typeof(App).Assembly.GetName();
                var basicFramework = basicFrameworkDescription.Name;
                var basicFrameworkVersion = basicFrameworkDescription.Version?.ToString();
                var environment = httpContext.RequestServices.GetRequiredService<IWebHostEnvironment>().EnvironmentName;
                var entryAssemblyName = Assembly.GetEntryAssembly()?.GetName().Name;
                // 获取进程信息
                var process = Process.GetCurrentProcess();
                var processName = process.ProcessName;

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
                    ,"━━━━━━━━━━━━━━━  Cookies ━━━━━━━━━━━━━━━"
                    , $"##请求端## {requestHeaderCookies}"
                    , $"##响应端## {responseHeaderCookies}"
                    ,"━━━━━━━━━━━━━━━  系统信息 ━━━━━━━━━━━━━━━"
                    , $"##系统名称## {osDescription}"
                    , $"##系统架构## {osArchitecture}"
                    , $"##基础框架## {basicFramework} v{basicFrameworkVersion}"
                    , $"##.NET 架构## {frameworkDescription}"
                    ,"━━━━━━━━━━━━━━━  启动信息 ━━━━━━━━━━━━━━━"
                    , $"##运行环境## {environment}"
                    , $"##启动程序集## {entryAssemblyName}"
                    , $"##进程名称## {processName}"
                };
                
                httpContext.Items.TryAdd(LogContextConst.Request_LoggerItems, loggerItems);
            };
        }
        else
        {
            _enrichAction = enrichAction;
        }
    }


    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var httpContext = _serviceProvider.GetService<IHttpContextAccessor>()?.HttpContext;
        if (null != httpContext)
        {
            _enrichAction.Invoke(logEvent, propertyFactory, httpContext);
        }
    }


    /// <summary>
    /// 处理泛型类型转字符串打印问题
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private static string HandleGenericType(Type type)
    {
        if (type == null) return string.Empty;

        var typeName = type.FullName ?? (!string.IsNullOrEmpty(type.Namespace) ? type.Namespace + "." : string.Empty) + type.Name;

        // 处理泛型类型问题
        if (type.IsConstructedGenericType)
        {
            var prefix = type.GetGenericArguments()
                .Select(genericArg => HandleGenericType(genericArg))
                .Aggregate((previous, current) => previous + ", " + current);

            typeName = typeName.Split('`').First() + "<" + prefix + ">";
        }

        return typeName;
    }

}


