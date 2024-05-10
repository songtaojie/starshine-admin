// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using AngleSharp.Html.Parser;
using AspNetCoreRateLimit;
using FluentEmail.Core;
using FreeRedis;
using Hx.Admin.IService;
using Hx.Admin.Models.ViewModels.Role;
using Lazy.Captcha.Core;
using Magicodes.ExporterAndImporter.Excel;
using Magicodes.ExporterAndImporter.Pdf;
using Microsoft.AspNetCore.Http;
using Nest;
using OnceMi.AspNetCore.OSS;
using SKIT.FlurlHttpClient.Wechat.Api;
using SKIT.FlurlHttpClient.Wechat.TenpayV3;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UAParser;
using Yitter.IdGenerator;

namespace Hx.Admin.Web.Entry.Controllers;

/// <summary>
/// 系统监控服务
/// </summary>
public class SysServerController : AdminControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public SysServerController(IHttpContextAccessor httpContextAccessor, 
        IWebHostEnvironment webHostEnvironment)
    {
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
    }

    /// <summary>
    /// 获取服务器配置信息
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public dynamic GetServerBase()
    {
        return new
        {
            HostName = Environment.MachineName, // 主机名称
            SystemOs = RuntimeInformation.OSDescription, // 操作系统
            OsArchitecture = Environment.OSVersion.Platform.ToString() + " " + RuntimeInformation.OSArchitecture.ToString(), // 系统架构
            ProcessorCount = Environment.ProcessorCount + " 核", // CPU核心数
            SysRunTime = ComputerUtil.GetRunTime(), // 系统运行时间
            RemoteIp = ComputerUtil.GetIpFromOnline(), // 外网地址
            LocalIp = _httpContextAccessor.HttpContext.Connection?.LocalIpAddress.ToString(), // 本地地址
            FrameworkDescription = RuntimeInformation.FrameworkDescription, // NET框架
            Environment = _webHostEnvironment.EnvironmentName,
            Wwwroot = _webHostEnvironment.WebRootPath, // 网站根目录
        };
    }

    /// <summary>
    /// 获取服务器使用信息
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public dynamic GetServerUsed()
    {
        var programStartTime = Process.GetCurrentProcess().StartTime;
        var totalMilliseconds = (DateTime.Now - programStartTime).TotalMilliseconds.ToString();
        var ts = totalMilliseconds.Contains('.') ? totalMilliseconds.Split('.')[0] : totalMilliseconds;
        long.TryParse(ts, out long time);
        var programRunTime = DateTimeUtil.FormatTime(time);

        var memoryMetrics = ComputerUtil.GetComputerInfo();
        return new
        {
            memoryMetrics.FreeRam, // 空闲内存
            memoryMetrics.UsedRam, // 已用内存
            memoryMetrics.TotalRam, // 总内存
            memoryMetrics.RamRate, // 内存使用率
            memoryMetrics.CpuRate, // Cpu使用率
            StartTime = programStartTime.ToString("yyyy-MM-dd HH:mm:ss"), // 服务启动时间
            RunTime = programRunTime, // 服务运行时间
        };
    }

    /// <summary>
    /// 获取服务器磁盘信息
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public dynamic GetServerDisk()
    {
        return ComputerUtil.GetDiskInfos();
    }

    /// <summary>
    /// 获取框架主要程序集
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public dynamic GetAssemblyList()
    {
        var sqlSugarAssembly = typeof(ISqlSugarClient).Assembly.GetName();
        var yitIdAssembly = typeof(YitIdHelper).Assembly.GetName();
        var redisAssembly = typeof(RedisClient).Assembly.GetName();
        var jsonAssembly = typeof(NewtonsoftJsonMvcCoreBuilderExtensions).Assembly.GetName();
        var excelAssembly = typeof(IExcelImporter).Assembly.GetName();
        var pdfAssembly = typeof(IPdfExporter).Assembly.GetName();
        var captchaAssembly = typeof(ICaptcha).Assembly.GetName();
        var wechatApiAssembly = typeof(WechatApiClient).Assembly.GetName();
        var wechatTenpayAssembly = typeof(WechatTenpayClient).Assembly.GetName();
        var ossAssembly = typeof(IOSSServiceFactory).Assembly.GetName();
        var parserAssembly = typeof(Parser).Assembly.GetName();
        var nestAssembly = typeof(IElasticClient).Assembly.GetName();
        var limitAssembly = typeof(IpRateLimitMiddleware).Assembly.GetName();
        var htmlParserAssembly = typeof(HtmlParser).Assembly.GetName();
        var fluentEmailAssembly = typeof(IFluentEmail).Assembly.GetName();

        return new[]
        {
            new { sqlSugarAssembly.Name, sqlSugarAssembly.Version },
            new { yitIdAssembly.Name, yitIdAssembly.Version },
            new { redisAssembly.Name, redisAssembly.Version },
            new { jsonAssembly.Name, jsonAssembly.Version },
            new { excelAssembly.Name, excelAssembly.Version },
            new { pdfAssembly.Name, pdfAssembly.Version },
            new { captchaAssembly.Name, captchaAssembly.Version },
            new { wechatApiAssembly.Name, wechatApiAssembly.Version },
            new { wechatTenpayAssembly.Name, wechatTenpayAssembly.Version },
            new { ossAssembly.Name, ossAssembly.Version },
            new { parserAssembly.Name, parserAssembly.Version },
            new { nestAssembly.Name, nestAssembly.Version },
            new { limitAssembly.Name, limitAssembly.Version },
            new { htmlParserAssembly.Name, htmlParserAssembly.Version },
            new { fluentEmailAssembly.Name, fluentEmailAssembly.Version },
        };
    }
}