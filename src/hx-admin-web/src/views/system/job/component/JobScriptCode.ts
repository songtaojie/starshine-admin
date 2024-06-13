export const JobScriptCode = `#region using

using System;
using System.Data;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Yitter.IdGenerator;
using Hx.Admin.Models;
using Hx.Sqlsugar;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

#endregion

namespace Hx.Admin.Tasks;

/// <summary>
/// 动态作业任务
/// </summary>
[JobDetail("你的作业编号",Description = "作业的描述", GroupName = "default")]
[DisallowConcurrentExecution]
public class DynamicJob : IJob
{
    private readonly IServiceProvider _serviceProvider;

    public DynamicJob(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Execute(IJobExecutionContext context)
    {

        // 请求网址
        // var result = await "http://www.baidu.com".GetAsStringAsync();
        // Console.WriteLine(result);
        await Task.CompletedTask;
    }
}`;
