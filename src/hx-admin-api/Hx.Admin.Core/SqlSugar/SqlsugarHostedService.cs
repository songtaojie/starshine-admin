// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Admin.Core;
/// <summary>
/// 监听泛型主机启动事件
/// </summary>
public class SqlSugarHostedService : IHostedService
{
    private readonly IHost _host;
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="host"></param>
    public SqlSugarHostedService(IHost host)
    {
        _host = host;
    }

    /// <summary>
    /// 监听主机启动
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        using (var serviceCope = _host.Services.CreateScope())
        {
            _ = serviceCope.ServiceProvider.GetService<ISqlSugarClient>();
        }
        return Task.CompletedTask;
    }

    /// <summary>
    /// 监听主机停止
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}