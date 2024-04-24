// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Magicodes.ExporterAndImporter.Excel.Utility.TemplateExport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
                var controllerName = httpContext.Request.RouteValues["controller"];
                var actionName = httpContext.Request.RouteValues["action"];
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("controllerName", controllerName));
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("actionName", actionName));
                if (httpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
                {
                    x_forwarded_for = httpContext.Request.Headers["X-Forwarded-For"];
                }
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("client_ip", JsonConvert.SerializeObject(x_forwarded_for)));
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("request_path", httpContext.Request.Path));
                // 获取服务端 IPv4 地址
                var localIPv4 = httpContext.GetLocalIpAddressToIPv4();
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(nameof(localIPv4), localIPv4));
                // 获取客户端 IPv4 地址
                var remoteIPv4 = httpContext.GetRemoteIpAddressToIPv4();
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(nameof(remoteIPv4), remoteIPv4));
                // 获取请求方式
                var requestMethod = httpContext.Request.Method;
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(nameof(requestMethod), requestMethod));

                // 客户端连接 ID
                var traceId = httpContext.TraceIdentifier;
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(nameof(traceId), traceId));

                // 线程 Id
                var threadId = Environment.CurrentManagedThreadId;
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(nameof(threadId), threadId));

                // 获取请求的 Url 地址
                var requestUrl = Uri.UnescapeDataString(httpContext.Request.GetRequestUrlAddress());
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(nameof(requestUrl), requestUrl));

                // 获取来源 Url 地址
                var refererUrl = Uri.UnescapeDataString(httpContext.Request.GetRefererUrlAddress());
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(nameof(refererUrl), refererUrl));

                // 客户端浏览器信息
                var userAgent = httpContext.Request.Headers["User-Agent"];
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(nameof(userAgent), userAgent));

                // 客户端请求区域语言
                var acceptLanguage = httpContext.Request.Headers["accept-language"];
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(nameof(acceptLanguage), acceptLanguage));

                // 请求来源（swagger还是其他）
                var requestFrom = httpContext.Request.Headers["request-from"].ToString();
                requestFrom = string.IsNullOrWhiteSpace(requestFrom) ? "client" : requestFrom;
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(nameof(requestFrom), requestFrom));

                // 获取授权用户
                var user = httpContext.User;

                // 获取请求 cookies 信息
                var requestHeaderCookies = Uri.UnescapeDataString(httpContext.Request.Headers["cookie"].ToString());
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(nameof(requestHeaderCookies), requestHeaderCookies));

                if (httpContext.Response.HasStarted)
                {
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("response_status", httpContext.Response.StatusCode));
                }
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


