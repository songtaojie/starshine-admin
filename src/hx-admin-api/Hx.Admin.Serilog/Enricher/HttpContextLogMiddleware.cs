// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Magicodes.ExporterAndImporter.Excel.Utility.TemplateExport;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nest;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Admin.Serilog.Enricher;
public class HttpContextLogMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    public HttpContextLogMiddleware(RequestDelegate next,Logger<HttpContextLogMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var serviceProvider = context.RequestServices;
        // 将我们自定义的Enricher添加到LogContext中。
        // LogContext功能很强大，可以动态添加属性，具体使用介绍，参见官方wiki文档
        using (LogContext.Push(new HttpContextEnricher(serviceProvider)))
        {
            var timeOperation = Stopwatch.StartNew();
            await _next(context);
            timeOperation.Stop();
            LogContext.PushProperty("ElapsedMilliseconds", timeOperation.ElapsedMilliseconds);
            _logger.LogInformation("请求接口");
        }
    }
}



