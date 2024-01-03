// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using OnceMi.AspNetCore.OSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection;
/// <summary>
/// ES服务注册
/// </summary>
public static class OSSServiceCollectionExtensions
{
    public static void AddOSSServiceSetup(this IServiceCollection services, IConfiguration configuration)
    {
        var isEnable = configuration.GetValue("OSSProvider:IsEnable", false);
        if (!isEnable) return;

        services.AddOSSService(configuration["OSSProvider:Provider"],options => { });

    }
}