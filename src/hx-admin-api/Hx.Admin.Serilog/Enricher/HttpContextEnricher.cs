// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Magicodes.ExporterAndImporter.Excel.Utility.TemplateExport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Nest;
using Newtonsoft.Json;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
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
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(LogContextConst.WriteToDb, true));
                if (httpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
                {
                    x_forwarded_for = httpContext.Request.Headers["X-Forwarded-For"];
                }
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("client_ip", JsonConvert.SerializeObject(x_forwarded_for)));
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("request_path", httpContext.Request.Path));
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("request_method", httpContext.Request.Method));
                if (httpContext.Response.HasStarted)
                {
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("response_status", httpContext.Response.StatusCode));
                }
                var actionDescriptor = httpContext.RequestServices.GetService<ControllerActionDescriptor>();
                // 获取正在处理的路由数据
                var routeData = httpContext.GetRouteData();
                var controllerName = routeData.Values["controller"];
                var actionName = routeData.Values["action"];
                var areaName = routeData.DataTokens["area"];
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(nameof(controllerName), controllerName?.ToString()));
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(nameof(actionName), actionName?.ToString()));
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(nameof(areaName), areaName?.ToString()));
                actionDescriptor ??= httpContext.GetControllerActionDescriptor();
                if (actionDescriptor != null)
                {
                    // 调用呈现链名称
                    var actionMethod = actionDescriptor.MethodInfo!;
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("controllerTypeName", actionMethod.DeclaringType?.Name));
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("controllerTypeName", actionMethod.Name));
                   
                    // 获取方法完整名称
                    var methodFullName = actionMethod.DeclaringType?.FullName + "." + actionMethod.Name;
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("displayName", methodFullName));
                    // [DisplayName] 特性
                    var displayNameAttribute = actionMethod.IsDefined(typeof(DisplayNameAttribute), true)
                        ? actionMethod.GetCustomAttribute<DisplayNameAttribute>(true)
                        : default;
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("displayTitle", displayNameAttribute?.DisplayName));
                }

                // 获取 HttpContext 和 HttpRequest 对象
                var httpRequest = httpContext.Request;

                // 获取服务端 IPv4 地址
                var localIPv4 = httpContext.GetLocalIpAddressToIPv4();
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(nameof(localIPv4), localIPv4));

                // 获取服务端源端口
                var localPort = httpContext.Connection.LocalPort;
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(nameof(localPort), localPort));

                // 获取客户端 IPv4 地址
                var remoteIPv4 = httpContext.GetRemoteIpAddressToIPv4();
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(nameof(remoteIPv4), remoteIPv4));

                // 获取客户端远程端口
                var remotePort = httpContext.Connection.RemotePort;
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(nameof(remotePort), remotePort));

                // 获取请求方式
                var httpMethod = httpContext.Request.Method;
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(nameof(httpMethod), httpMethod));

                // 客户端连接 ID
                var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier ;
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(nameof(traceId), traceId));

                // 线程 Id
                var threadId = Environment.CurrentManagedThreadId;
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(nameof(threadId), threadId));

                // 获取请求的 Url 地址
                var requestUrl = Uri.UnescapeDataString(httpRequest.GetRequestUrlAddress());
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(nameof(requestUrl), requestUrl));

                // 获取来源 Url 地址
                var refererUrl = Uri.UnescapeDataString(httpRequest.GetRefererUrlAddress());
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(nameof(refererUrl), refererUrl));

                // 客户端浏览器信息
                var userAgent = httpRequest.Headers["User-Agent"];
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(nameof(userAgent), userAgent));

                // 客户端请求区域语言
                var acceptLanguage = httpRequest.Headers["accept-language"];
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(nameof(acceptLanguage), acceptLanguage));

                // 请求来源（swagger还是其他）
                var requestFrom = httpRequest.Headers["request-from"].ToString();
                requestFrom = string.IsNullOrWhiteSpace(requestFrom) ? "client" : requestFrom;
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(nameof(requestFrom), requestFrom));

                // 获取授权用户
                var user = httpContext.User;

                // 获取请求 cookies 信息
                var requestHeaderCookies = Uri.UnescapeDataString(httpRequest.Headers["cookie"].ToString());
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(nameof(requestHeaderCookies), requestHeaderCookies));

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
}


