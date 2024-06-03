// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Elastic.Clients.Elasticsearch.IndexManagement;
using Hx.Admin.Core;
using Hx.Sqlsugar;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Impl.AdoJobStore;
using Quartz.Spi;
using System.Collections.Concurrent;
using System.Data.Common;

namespace Hx.Admin.Tasks.JobStore;
public class DbJobStoreTX : JobStoreTX
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly DbSettingsOptions _dbSettingsOptions;
    public DbJobStoreTX(IWebHostEnvironment webHostEnvironment,
        IOptions<DbSettingsOptions> options)
    {
        _webHostEnvironment = webHostEnvironment;
        _dbSettingsOptions = options.Value;
    }
    public override async Task Initialize(ITypeLoadHelper loadHelper, ISchedulerSignaler signaler, CancellationToken cancellationToken = default)
    {
        await base.Initialize(loadHelper, signaler, cancellationToken);
    }
}
