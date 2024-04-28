// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Sdk.Core;
using Magicodes.ExporterAndImporter.Excel.Utility.TemplateExport;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Nest;
using Newtonsoft.Json;
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
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hx.Admin.Serilog.Enricher;
public class HttpContextEnricher : ILogEventEnricher
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Action<LogEvent, ILogEventPropertyFactory, HttpContext> _enrichAction;
    /// <summary>
    /// 模板正则表达式对象
    /// </summary>
    private static readonly Lazy<Regex> _lazyRegex = new(() => new(@"^##(?<prop>.*)?##[:：]?\s*(?<content>[\s\S]*)"));

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

                if (httpContext.User != null && httpContext.User.Identity != null && httpContext.User.Identity.IsAuthenticated)
                {
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(LogContextConst.Request_Claims, httpContext.User.Claims));
                }
                // 获取请求 cookies 信息
                var requestHeaderCookies = Uri.UnescapeDataString(httpRequest.Headers["cookie"].ToString());
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(LogContextConst.Request_HeaderCookies, requestHeaderCookies));

                //请求时间
                if (httpContext.Items.TryGetValue(LogContextConst.Request_ElapsedMilliseconds, out object? elapsedMilliseconds))
                {
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(LogContextConst.Request_ElapsedMilliseconds, elapsedMilliseconds));
                }
                //请求结果
                if (httpContext.Items.TryGetValue(LogContextConst.Route_ActionResult, out object? actionResult))
                {
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(LogContextConst.Route_ActionResult, actionResult));
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

                var monitorItems = new List<string>()
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
                    , $"##执行耗时## {elapsedMilliseconds}ms"
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

                var finalMessage = Wrapper("请求日志", displayName, monitorItems.ToArray());

                httpContext.Items.Add(LogContextConst.FinalMessage, finalMessage);
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

    private string Wrapper(string title, string? description, params string[] items)
    {
        // 处理不同编码问题
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        var stringBuilder = new StringBuilder();
        stringBuilder.Append($"┏━━━━━━━━━━━  {title} ━━━━━━━━━━━").AppendLine();

        // 添加描述
        if (!string.IsNullOrWhiteSpace(description))
        {
            stringBuilder.Append($"┣ {description}").AppendLine().Append("┣ ").AppendLine();
        }

        // 添加项
        if (items != null && items.Length > 0)
        {
            var propMaxLength = items.Where(u => _lazyRegex.Value.IsMatch(u))
                .DefaultIfEmpty(string.Empty)
                .Max(u => _lazyRegex.Value.Match(u).Groups["prop"].Value.Length);

            // 控制项名称对齐空白占位数
            propMaxLength += (propMaxLength >= 5 ? 10 : 5);

            // 遍历每一项并进行正则表达式匹配
            for (var i = 0; i < items.Length; i++)
            {
                var item = items[i];

                // 判断是否匹配 ##xxx##
                if (_lazyRegex.Value.IsMatch(item))
                {
                    var match = _lazyRegex.Value.Match(item);
                    var prop = match.Groups["prop"].Value;
                    var content = match.Groups["content"].Value;

                    var propTitle = $"{prop}：";
                    stringBuilder.Append($"┣ {PadRight(propTitle, propMaxLength)}{content}").AppendLine();
                }
                else
                {
                    stringBuilder.Append($"┣ {item}").AppendLine();
                }
            }
        }

        stringBuilder.Append($"┗━━━━━━━━━━━  {title} ━━━━━━━━━━━");
        return stringBuilder.ToString();
    }


    /// <summary>
    /// 等宽文字对齐
    /// </summary>
    /// <param name="str"></param>
    /// <param name="totalByteCount"></param>
    /// <returns></returns>
    private string PadRight(string str, int totalByteCount)
    {
        var coding = Encoding.GetEncoding("gbk");
        var dcount = 0;

        foreach (var character in str.ToCharArray())
        {
            if (coding.GetByteCount(character.ToString()) == 2)
                dcount++;
        }

        var w = str.PadRight(totalByteCount - dcount);
        return w;
    }
}


