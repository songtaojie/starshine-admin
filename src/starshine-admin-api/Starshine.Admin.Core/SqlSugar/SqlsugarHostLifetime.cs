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

namespace Starshine.Admin.Core;
public class SqlsugarHostLifetime : IHostLifetime
{
    private readonly IServiceProvider _serviceProvider;
    public SqlsugarHostLifetime(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task WaitForStartAsync(CancellationToken cancellationToken)
    {
        using (var serviceCope = _serviceProvider.CreateScope())
        {
            _ = serviceCope.ServiceProvider.GetService<ISqlSugarClient>();
        }
        return Task.CompletedTask;
    }
}
