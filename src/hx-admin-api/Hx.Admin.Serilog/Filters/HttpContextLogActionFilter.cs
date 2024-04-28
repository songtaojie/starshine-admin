// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Admin.Serilog.Enricher;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System.Diagnostics;
using System;
using System.Text.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.Text;
using Nest;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.ComponentTCBDescribeCloudBaseRunEnvironmentsResponse.Types;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.ProductSPUUpdateRequest.Types;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.ShopCouponGetResponse.Types.Result.Types.Coupon.Types.CouponDetail.Types.Discount.Types.DiscountCondidtion.Types;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;
using UAParser;
using Microsoft.AspNetCore.Http;

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
        // 计算接口执行时间
        var timeOperation = Stopwatch.StartNew();
        var resultContext = await next();
        timeOperation.Stop();
        context.HttpContext.Items.Add(LogContextConst.Request_ElapsedMilliseconds, timeOperation.ElapsedMilliseconds);
        if (resultContext != null && resultContext.Result != null)
        {
            context.HttpContext.Items.Add(LogContextConst.Route_ActionResult, GenerateReturnInfomation(resultContext));
        }
        var serviceProvider = context.HttpContext.RequestServices;
        var logger = serviceProvider.GetRequiredService<ILogger<HttpContextLogActionFilter>>();
        using (LogContext.Push(new HttpContextEnricher(serviceProvider)))
        {
            if (context.HttpContext.Items.TryGetValue(LogContextConst.FinalMessage, out object? value) && value !=null)
            {
                logger.LogInformation(value.ToString());
            }
        }
    }
    

    /// <summary>
    /// 生成返回值信息
    /// </summary>
    /// <param name="resultContext"></param>
    /// <param name="method"></param>
    /// <returns></returns>
    private string GenerateReturnInfomation(ActionExecutedContext resultContext)
    {
        object? returnValue = null;
        string returnTypeName = string.Empty;
        // 解析返回值
        if (CheckVaildResult(resultContext.Result, out var data))
        {
            returnValue = data;
            returnTypeName = HandleGenericType(data!.GetType());
        }
        // 处理文件类型
        else if (resultContext.Result is FileResult fileResult)
        {
            returnValue = new
            {
                FileName = fileResult.FileDownloadName,
                fileResult.ContentType,
                Length = fileResult is FileContentResult cresult ? (object)cresult.FileContents.Length : null
            };
            returnTypeName = HandleGenericType(fileResult!.GetType());
        }
        else if (resultContext.Result != null)
        {
            returnTypeName = HandleGenericType(resultContext.Result.GetType());
        }
        else
        {
            return string.Empty;
        }
        var actionMethod = (resultContext.ActionDescriptor as ControllerActionDescriptor)?.MethodInfo;
        if (string.IsNullOrEmpty(returnTypeName) && actionMethod != null)
        {
            returnTypeName = HandleGenericType(actionMethod.ReturnType);
        }
        return JsonSerializer.Serialize(new { ReturnType = returnTypeName, StatusCode = resultContext.HttpContext.Response.StatusCode, Result = returnValue });
    }


    /// <summary>
    /// 检查是否是有效的结果
    /// </summary>
    /// <param name="result"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    private bool CheckVaildResult(IActionResult? result, out object? data)
    {
        data = default;

        // 排除以下结果，跳过规范化处理
        var isDataResult = result switch
        {
            ViewResult => false,
            PartialViewResult => false,
            FileResult => false,
            ChallengeResult => false,
            SignInResult => false,
            SignOutResult => false,
            RedirectToPageResult => false,
            RedirectToRouteResult => false,
            RedirectResult => false,
            RedirectToActionResult => false,
            LocalRedirectResult => false,
            ForbidResult => false,
            ViewComponentResult => false,
            PageResult => false,
            NotFoundResult => false,
            NotFoundObjectResult => false,
            _ => true,
        };

        // 目前支持返回值 ActionResult
        if (isDataResult) data = result switch
        {
            // 处理内容结果
            ContentResult content => content.Content,
            // 处理对象结果
            ObjectResult obj => obj.Value,
            // 处理 JSON 对象
            JsonResult json => json.Value,
            _ => null,
        };

        return isDataResult;
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
