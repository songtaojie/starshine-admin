// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Serilog.Context;
using SqlSugar;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Xml.XPath;
using System.Text;
using System.Text.RegularExpressions;
using Starshine.Extensions;
using Starshine.JsonSerialization;

namespace Starshine.Admin.Serilog.Enricher;
public class HttpContextLogScope : IDisposable
{
    private static readonly Lazy<Regex> _lazyRegex = new(() => new(@"^##(?<prop>.*)?##[:：]?\s*(?<content>[\s\S]*)"));
    private readonly Stack<IDisposable> _disposableStack = new Stack<IDisposable>();
    private string _displayName = string.Empty;
    private readonly List<string> _loggerItems = new List<string>();

    public static HttpContextLogScope Instance => new();

    public HttpContextLogScope AddStock(IDisposable disposable)
    {
         _disposableStack.Push(disposable);
        return this;
    }

    public HttpContextLogScope AddStock(string name,object? value)
    {
        AddStock(LogContext.PushProperty(name, value));
        return this;
    }

    public HttpContextLogScope PushProperty(ActionExecutedContext resultContext)
    {
        
        var httpContext = resultContext.HttpContext;
        var x_forwarded_for = new StringValues();
        if (httpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
        {
            x_forwarded_for = httpContext.Request.Headers["X-Forwarded-For"];
        }
        AddStock(LogContextConst.Client_Ip, JsonSerializer.Serialize(x_forwarded_for));
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
        var actionDescriptor = resultContext.ActionDescriptor as ControllerActionDescriptor;
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
            _displayName = displayName;
            AddStock(LogContextConst.Route_DisplayName, displayName);
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
        AddStock(LogContextConst.Request_ThreadId, threadId);

        // 获取请求的 Url 地址
        var requestUrl = Uri.UnescapeDataString(httpRequest.GetRequestUrlAddress());
        AddStock(LogContextConst.Request_FullUrl, requestUrl);

        // 获取来源 Url 地址
        var refererUrl = Uri.UnescapeDataString(httpRequest.GetRefererUrlAddress());
        AddStock(LogContextConst.Request_RefererUrl, refererUrl);

        // 客户端浏览器信息
        var userAgent = httpRequest.Headers["User-Agent"].ToString();
        AddStock(LogContextConst.Request_UserAgent, userAgent);

        // 客户端请求区域语言
        var acceptLanguage = httpRequest.Headers["accept-language"].ToString();
        AddStock(LogContextConst.Request_AcceptLanguage, acceptLanguage);

        // 请求来源（swagger还是其他）
        var requestFrom = httpRequest.Headers["request-from"].ToString();
        requestFrom = string.IsNullOrWhiteSpace(requestFrom) ? "client" : requestFrom;
        AddStock(LogContextConst.Request_RequestFrom, requestFrom);

        // 获取请求 cookies 信息
        var requestHeaderCookies = Uri.UnescapeDataString(httpRequest.Headers["cookie"].ToString());
        AddStock(LogContextConst.Request_HeaderCookies, requestHeaderCookies);
        // 获取响应状态码
        AddStock(LogContextConst.Response_StatusCode, httpContext.Response.StatusCode);
        
        // 生成请求头日志模板
        if (httpContext.Items.TryGetValue(LogContextConst.Request_ElapsedMilliseconds, out object? elapsedMilliseconds))
        {
            AddStock(LogContextConst.Request_ElapsedMilliseconds, elapsedMilliseconds);
        }
        var basicLoggerItems = new List<string>()
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
        };
        _loggerItems.AddRange(basicLoggerItems);
        _loggerItems.AddRange(GenerateRequestHeadersTemplate(httpContext.Request.Headers));
        _loggerItems.AddRange(GenerateCookiesTemplate(httpContext));
        _loggerItems.AddRange(GenerateSystemTemplate());
        _loggerItems.AddRange(GenerateProcessTemplate(httpContext));

        if (httpContext.User != null && httpContext.User.Identity != null && httpContext.User.Identity.IsAuthenticated)
        {
            var accessToken = httpContext.Response.Headers["access-token"].ToString();
            string? authorization = string.IsNullOrWhiteSpace(accessToken)
                    ? httpContext.Request.Headers["Authorization"].ToString()
                    : "Bearer " + accessToken;
            AddStock(LogContextConst.Request_Authorization, authorization);
            List<object> authorizationClaims;
            _loggerItems.AddRange(GenerateAuthorizationTemplate(httpContext.User, authorization, out authorizationClaims));
            if (authorizationClaims.Any())
            {
                AddStock(LogContextConst.Request_Claims, JsonSerializer.Serialize(authorizationClaims));
            }
        }
        if (actionDescriptor != null)
        {
            var actionMethod = actionDescriptor.MethodInfo;
            if (httpContext.Items.TryGetValue(LogContextConst.Route_ActionParameters, out object? parameterValues) && parameterValues is IDictionary<string, object?> actionArguments)
            {
                _loggerItems.AddRange(GenerateParameterTemplate(actionArguments, actionMethod, httpContext.Request.Headers["Content-Type"], out List<object> paramsList));
                AddStock(LogContextConst.Route_ActionParameters, JsonSerializer.Serialize(paramsList));
            }
            // 添加返回值信息日志模板
            _loggerItems.AddRange(GenerateReturnInfomationTemplate(resultContext, actionMethod, out string? returnValue));
            AddStock(LogContextConst.Route_ActionResult, returnValue);
        }
        return this;
    }

    public string? GetFinalMessage()
    {
        return Wrapper("请求日志", _displayName, _loggerItems.ToArray());
    }
    #region 生成日志消息
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
    #endregion

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

    #region 消息模板
    /// <summary>
    /// 生成Cookies日志模板
    /// </summary>
    /// <param name="headers"></param>
    /// <returns></returns>
    private List<string> GenerateCookiesTemplate(HttpContext httpContext)
    {
        // 获取请求 cookies 信息
        var requestHeaderCookies = Uri.UnescapeDataString(httpContext.Request.Headers["cookie"].ToString());
        // 获取响应 cookies 信息
        var responseHeaderCookies = Uri.UnescapeDataString(httpContext.Response.Headers["Set-Cookie"].ToString());
        return new List<string>()
        {
            "━━━━━━━━━━━━━━━  Cookies ━━━━━━━━━━━━━━━"
            , $"##请求端## {requestHeaderCookies}"
            , $"##响应端## {responseHeaderCookies}"
        };
    }

    /// <summary>
    /// 生成系统信息日志模板
    /// </summary>
    /// <returns></returns>
    private List<string> GenerateSystemTemplate()
    {
        var osDescription = RuntimeInformation.OSDescription;
        var osArchitecture = RuntimeInformation.OSArchitecture.ToString();
        var frameworkDescription = RuntimeInformation.FrameworkDescription;
        var basicFrameworkDescription = typeof(StarshineApp).Assembly.GetName();
        var basicFramework = basicFrameworkDescription.Name;
        var basicFrameworkVersion = basicFrameworkDescription.Version?.ToString();
        return new List<string>()
        {
             "━━━━━━━━━━━━━━━  系统信息 ━━━━━━━━━━━━━━━"
            , $"##系统名称## {osDescription}"
            , $"##系统架构## {osArchitecture}"
            , $"##基础框架## {basicFramework} v{basicFrameworkVersion}"
            , $"##.NET 架构## {frameworkDescription}"
        };
    }
    /// <summary>
    /// 生成启动信息日志模板
    /// </summary>
    /// <returns></returns>
    private List<string> GenerateProcessTemplate(HttpContext httpContext)
    {
        var environment = httpContext.RequestServices.GetRequiredService<IWebHostEnvironment>().EnvironmentName;
        var entryAssemblyName = Assembly.GetEntryAssembly()?.GetName().Name;
        // 获取进程信息
        var process = Process.GetCurrentProcess();
        var processName = process.ProcessName;
        return new List<string>()
        {
            "━━━━━━━━━━━━━━━  启动信息 ━━━━━━━━━━━━━━━"
            , $"##运行环境## {environment}"
            , $"##启动程序集## {entryAssemblyName}"
            , $"##进程名称## {processName}"
        };
    }


    /// <summary>
    /// 生成请求头日志模板
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="headers"></param>
    /// <returns></returns>
    private List<string> GenerateRequestHeadersTemplate(IHeaderDictionary headers)
    {
        var templates = new List<string>();

        if (!headers.Any()) return templates;

        templates.AddRange(new[]
        {
            $"━━━━━━━━━━━━━━━  请求头信息 ━━━━━━━━━━━━━━━"
            , $""
        });

        // 遍历请求头列表
        foreach (var (key, value) in headers)
        {
            templates.Add($"##{key}## {value}");
        }
        return templates;
    }


    /// <summary>
    /// 生成 JWT 授权信息日志模板
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="claimsPrincipal"></param>
    /// <param name="authorization"></param>
    /// <returns></returns>
    private List<string> GenerateAuthorizationTemplate(ClaimsPrincipal claimsPrincipal, StringValues authorization, out List<object> authorizationClaims)
    {
        var templates = new List<string>();
        authorizationClaims = new List<object>();
        if (!claimsPrincipal.Claims.Any()) return templates;

        templates.AddRange(new[]
        {
            $"━━━━━━━━━━━━━━━  授权信息 ━━━━━━━━━━━━━━━"
            , $"##JWT Token## {authorization}"
            , $""
        });

        // 遍历身份信息
        foreach (var claim in claimsPrincipal.Claims)
        {
            var valueType = claim.ValueType.Replace("http://www.w3.org/2001/XMLSchema#", "");
            var value = claim.Value;

            // 解析时间戳并转换
            if (!string.IsNullOrEmpty(value) && (claim.Type == "iat" || claim.Type == "nbf" || claim.Type == "exp"))
            {
                var succeed = long.TryParse(value, out var seconds);
                if (succeed)
                {
                    value = $"{value} ({DateTimeOffset.FromUnixTimeSeconds(seconds).ToLocalTime():yyyy-MM-dd HH:mm:ss:ffff(zzz) dddd} L)";
                }
            }

            templates.Add($"##{claim.Type} ({valueType})## {value}");
            authorizationClaims.Add(new
            {
                claim.Type,
                ValueType = valueType,
                Value = value
            });
        }
        return templates;
    }



    /// <summary>
    /// 生成请求参数信息日志模板
    /// </summary>
    /// <param name="parameterValues"></param>
    /// <param name="method"></param>
    /// <param name="contentType"></param>
    /// <returns></returns>
    private List<string> GenerateParameterTemplate(IDictionary<string, object?> parameterValues, MethodInfo method, StringValues contentType, out List<object> paramsList)
    {
        var templates = new List<string>();
        paramsList = new List<object>();
        if (parameterValues == null || parameterValues.Count == 0)
        {
            return templates;
        }

        templates.AddRange(new[]
        {
            $"━━━━━━━━━━━━━━━  参数列表 ━━━━━━━━━━━━━━━"
            , $"##Content-Type## {contentType}"
            , $""
        });

        var parameters = method.GetParameters();

        foreach (var parameter in parameters)
        {
            // 排除标记 [FromServices] 的解析
            if (parameter.IsDefined(typeof(FromServicesAttribute), false)) continue;

            var name = parameter.Name!;
            var parameterType = parameter.ParameterType;

            _ = parameterValues.TryGetValue(name, out var value);

            object? rawValue = default;

            // 文件类型参数
            if (value is IFormFile || value is List<IFormFile>)
            {
                // 单文件
                if (value is IFormFile formFile)
                {
                    var fileSize = Math.Round(formFile.Length / 1024D);
                    templates.Add($"##{name} ({parameterType.Name})## [name]: {formFile.FileName}; [size]: {fileSize}KB; [content-type]: {formFile.ContentType}");
                    paramsList.Add(new
                    {
                        name,
                        type = HandleGenericType(parameterType),
                        value = new
                        {
                            formFile.Name,
                            formFile.FileName,
                            formFile.Length,
                            formFile.ContentType
                        }
                    });
                }
                // 多文件
                else if (value is List<IFormFile> formFiles)
                {
                    var valueArr = new List<object>();
                    for (var i = 0; i < formFiles.Count; i++)
                    {
                        var file = formFiles[i];
                        var size = Math.Round(file.Length / 1024D);
                        templates.Add($"##{name}[{i}] ({nameof(IFormFile)})## [name]: {file.FileName}; [size]: {size}KB; [content-type]: {file.ContentType}");
                        valueArr.Add(new
                        {
                            file.Name,
                            file.FileName,
                            file.Length,
                            file.ContentType
                        });
                    }
                    paramsList.Add(new
                    {
                        name,
                        type = HandleGenericType(parameterType),
                        value = valueArr
                    });
                }
            }
            // 处理 byte[] 参数类型
            else if (value is byte[] byteArray)
            {
                templates.Add($"##{name} ({parameterType.Name})## [Length]: {byteArray.Length}");
                paramsList.Add(new
                {
                    name,
                    type = HandleGenericType(parameterType),
                    value = new
                    {
                        byteArray.Length
                    }
                });
            }
            // 处理基元类型，字符串类型和空值
            else if (parameterType.IsPrimitive || value is string || value == null)
            {
                rawValue = value;
                paramsList.Add(new
                {
                    name,
                    type = HandleGenericType(parameterType),
                    value
                });
            }
            // 其他类型统一进行序列化
            else
            {
                rawValue = TrySerializeObject(value, out var succeed);
                paramsList.Add(new
                {
                    name,
                    type = HandleGenericType(parameterType),
                    value
                });
            }

            templates.Add($"##{name} ({parameterType.Name})## {rawValue}");

        }

        return templates;
    }

    /// <summary>
    /// 生成返回值信息日志模板
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="resultContext"></param>
    /// <param name="method"></param>
    /// <param name="monitorMethod"></param>
    /// <returns></returns>
    private List<string> GenerateReturnInfomationTemplate(ActionExecutedContext resultContext, MethodInfo method, out string? finalReturnValue)
    {
        var templates = new List<string>();

        object? returnValue = null;
        Type? finalReturnType;
        var result = resultContext.Result;

        // 解析返回值
        if (CheckVaildResult(result, out var data))
        {
            returnValue = data;
            finalReturnType = data?.GetType();
        }
        // 处理文件类型
        else if (result is FileResult fresult)
        {
            returnValue = new
            {
                FileName = fresult.FileDownloadName,
                fresult.ContentType,
                Length = fresult is FileContentResult cresult ? (object)cresult.FileContents.Length : null
            };
            finalReturnType = fresult?.GetType();
        }
        else finalReturnType = result?.GetType();

        // 获取最终呈现值（字符串类型）
        var displayValue = TrySerializeObject(returnValue, out var succeed);
        finalReturnValue = displayValue;
        var returnTypeName = HandleGenericType(method.ReturnType);
        var finalReturnTypeName = finalReturnType != null ? HandleGenericType(finalReturnType) : string.Empty;

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

        return templates;
    }


    /// <summary>
    /// 序列化对象
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="monitorMethod"></param>
    /// <param name="succeed"></param>
    /// <returns></returns>
    private string? TrySerializeObject(object? obj, out bool succeed)
    {
        if (obj == null)
        {
            succeed = true;
            return "{}";
        }
        // 排除 IQueryable<> 泛型
        if (obj != null && obj.GetType().HasImplementedRawGeneric(typeof(IQueryable<>)))
        {
            succeed = true;
            return obj!.ToString();
        }

        try
        {

            // 序列化默认配置
            var jsonSerializerOptions = new JsonSerializerOptions()
            {
                // 解决循环引用问题
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
            };
            jsonSerializerOptions.Converters.Add(new SystemTextJsonDateTimeJsonConverter());
            jsonSerializerOptions.Converters.Add(new SystemTextJsonNullableDateTimeJsonConverter());
            // 解决 long 精度问题
            jsonSerializerOptions.Converters.Add(new SystemTextJsonLongToStringJsonConverter());
            var result = System.Text.Json.JsonSerializer.Serialize(obj, jsonSerializerOptions);
            succeed = true;
            return result;
        }
        catch
        {
            succeed = true;
            return obj!.ToString();
        }
    }


    /// <   summary>
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
    #endregion

}
