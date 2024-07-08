using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Starshine.Admin.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// ES服务注册
/// </summary>
public static class ElasticSearchServiceCollection
{
    public static async void AddElasticSearch(this IServiceCollection services,IConfiguration configuration)
    {
        var enabled = configuration.GetValue("Logging:ElasticSearch:Enabled",false);
        if (!enabled) return;

        var serverUris = configuration.GetValue<List<string>>("Logging:ElasticSearch:ServerUris");
        var defaultIndex = configuration.GetValue<string>("Logging:ElasticSearch:DefaultIndex");
        //var client = new ElasticsearchClient("<CLOUD_ID>", new ApiKey("<API_KEY>"));
        var uris = serverUris.Select(u => new Uri(u));
        var connectionPool = new SniffingNodePool(uris);
        var settings = new ElasticsearchClientSettings(connectionPool).DefaultIndex(defaultIndex)
            .CertificateFingerprint("<FINGERPRINT>")
            .Authentication(new BasicAuthentication("<USERNAME>", "<PASSWORD>"));

        var client = new ElasticsearchClient(settings);
        //
        //var connectionPool = new SniffingConnectionPool(uris);
        //var settings = new ConnectionSettings(connectionPool).DefaultIndex(defaultIndex);
        //var client = new ElasticClient(settings);
        //i.Mappings<SysLogOp>(m => m.AutoMap())
        await client.Indices.CreateAsync(defaultIndex);

        services.AddSingleton(client); // 单例注册
    }
}