// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hx.Admin.Serilog.Filters;
/// <summary>
/// 日志记录
/// </summary>
public class HttpContextLogActionFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var resultContext = await next();
        if (resultContext != null && resultContext.Result != null) 
        {
            context.HttpContext.Items[LogContextConst.Route_ActionResult] = resultContext.Result;
        }
    }


    /// <summary>
    /// 生成返回值信息日志模板
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="resultContext"></param>
    /// <param name="method"></param>
    /// <param name="monitorMethod"></param>
    /// <returns></returns>
    private List<string> GenerateReturnInfomationTemplate(ActionExecutedContext resultContext, MethodInfo method)
    {
        var templates = new List<string>();

        object? returnValue = null;
        Type finalReturnType;
        // 解析返回值
        if (CheckVaildResult(resultContext.Result, out var data))
        {
            returnValue = data;
            finalReturnType = data!.GetType();
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
            finalReturnType = fileResult!.GetType();
        }
        else if (resultContext.Result != null)
        {
            finalReturnType = resultContext.Result.GetType();
        }
        else
        {
            return;
        }

        // 获取最终呈现值（字符串类型）
        var displayValue = TrySerializeObject(returnValue, monitorMethod, out var succeed);
        var originValue = displayValue;

        // 获取返回值阈值
        var threshold = GetReturnValueThreshold(monitorMethod);
        if (threshold > 0)
        {
            displayValue = displayValue.Length <= threshold ? displayValue : displayValue[..threshold];
        }

        var returnTypeName = HandleGenericType(method.ReturnType);
        var finalReturnTypeName = HandleGenericType(finalReturnType);

        // 获取请求返回的响应状态码
        var httpStatusCode = (resultContext as FilterContext).HttpContext.Response.StatusCode;

        templates.AddRange(new[]
        {
            $"━━━━━━━━━━━━━━━  返回信息 ━━━━━━━━━━━━━━━"
            , $"##HTTP响应状态码## {httpStatusCode}"
            , $"##原始类型## {returnTypeName}"
            , $"##最终类型## {finalReturnTypeName}"
            , $"##最终返回值## {displayValue}"
        });

        writer.WritePropertyName("returnInformation");
        writer.WriteStartObject();
        writer.WriteString("type", finalReturnTypeName);
        writer.WriteNumber(nameof(httpStatusCode), httpStatusCode);
        writer.WriteString("actType", returnTypeName);
        writer.WritePropertyName("value");
        if (succeed && method.ReturnType != typeof(void) && returnValue != null)
        {
            // 解决返回值被截断后 json 验证失败异常问题
            if (threshold > 0 && originValue != displayValue)
            {
                writer.WriteStringValue(displayValue);
            }
            else writer.WriteRawValue(displayValue);
        }
        else writer.WriteNullValue();

        writer.WriteEndObject();

        return templates;
    }


    /// <summary>
    /// 检查是否是有效的结果（可进行规范化的结果）
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
}
