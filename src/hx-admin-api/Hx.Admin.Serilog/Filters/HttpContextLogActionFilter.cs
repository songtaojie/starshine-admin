// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Admin.Serilog.Enricher;
using Hx.Common.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Serilog.Context;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Hx.Sdk.Core;
using System.Text.Json.Serialization;
using Hx.Common;
using Hx.Admin.Core;
using Hx.Admin.Serilog.Attributes;

namespace Hx.Admin.Serilog.Filters;
/// <summary>
/// 日志记录
/// </summary>
public class HttpContextLogActionFilter : IAsyncActionFilter
{
    public HttpContextLogActionFilter()
    {
    }
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {

        // 获取动作方法描述器
        var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
        if (actionDescriptor != null)
        {
            var hasSkipLogging = actionDescriptor.ControllerTypeInfo.HasAttribute<SkipLoggingAttribute>();
            if (hasSkipLogging)
            {
                await next();
                return;
            }
            var actionSkipLoggingAttribute = actionDescriptor.MethodInfo.GetCustomAttribute<SkipLoggingAttribute>();
            if (actionSkipLoggingAttribute != null)
            {
                await next();
                return;
            }
        }
        // 计算接口执行时间
        var timeOperation = Stopwatch.StartNew();
        var resultContext = await next();
        timeOperation.Stop();
        context.HttpContext.Items.TryAdd(LogContextConst.Request_ElapsedMilliseconds, timeOperation.ElapsedMilliseconds);
        context.HttpContext.Items.TryAdd(LogContextConst.Route_ActionParameters, context.ActionArguments);
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<HttpContextLogActionFilter>>();
        using var logScope = HttpContextLogScope.Instance.PushProperty(resultContext);
        // 写入日志，如果没有异常默认使用 LogInformation，否则使用 LogError
        if (resultContext.Exception == null)
        {
            logger.LogInformation(logScope.GetFinalMessage());
        }
        else
        {
            // 如果不是验证异常，写入 Error
            logger.LogError(resultContext.Exception, logScope.GetFinalMessage());
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
