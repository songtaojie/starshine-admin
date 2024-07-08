// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Starshine.Admin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection;

public static class WorkerServiceCollectionExtension
{
    public static IServiceCollection AddYitterIdGenerater(this IServiceCollection services)
    {
        return services.AddSingleton<WorkerNode>()
            .AddHostedService<WorkerNodeHostedService>();
    }
}
