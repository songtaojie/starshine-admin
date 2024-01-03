using Elasticsearch.Net;
using Hx.Admin.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// ES服务注册
/// </summary>
public static class ElasticSearchServiceCollection
{
    public static void AddElasticSearch(this IServiceCollection services,IConfiguration configuration)
    {
        var enabled = configuration.GetValue("Logging:ElasticSearch:Enabled",false);
        if (!enabled) return;

        var serverUris = configuration.GetValue<List<string>>("Logging:ElasticSearch:ServerUris");
        var defaultIndex = configuration.GetValue<string>("Logging:ElasticSearch:DefaultIndex");

        var uris = serverUris.Select(u => new Uri(u));
        var connectionPool = new SniffingConnectionPool(uris);
        var settings = new ConnectionSettings(connectionPool).DefaultIndex(defaultIndex);
        var client = new ElasticClient(settings);
        client.Indices.Create(defaultIndex, i => i.Map<SysLogOp>(m => m.AutoMap()));

        services.AddSingleton(client); // 单例注册
    }
}