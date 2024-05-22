// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Dm;
using Hx.Admin.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Quartz;
using Quartz.Impl.AdoJobStore;
using Quartz.Spi;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Admin.Tasks;
public class MyJobStoreTX: JobStoreTX
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ILogger<MyJobStoreTX> _logger;
    private readonly IServiceProvider _serviceProvider;
    public MyJobStoreTX(ILogger<MyJobStoreTX> logger,
        IWebHostEnvironment webHostEnvironment,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _webHostEnvironment = webHostEnvironment;
        _serviceProvider = serviceProvider;
    }
    public override async Task Initialize(ITypeLoadHelper loadHelper, ISchedulerSignaler signaler, CancellationToken cancellationToken = default)
    {
        await base.Initialize(loadHelper, signaler, cancellationToken);
    }
    protected override Task<int> GetNumberOfJobs(ConnectionAndTransactionHolder conn, CancellationToken cancellationToken = default)
    {
        return base.GetNumberOfJobs(conn, cancellationToken);
    }

    protected override Task<IReadOnlyCollection<string>> GetJobGroupNames(ConnectionAndTransactionHolder conn, CancellationToken cancellationToken = default)
    {
        return base.GetJobGroupNames(conn, cancellationToken);
    }
    protected override Task RecoverJobs(CancellationToken cancellationToken)
    {
        return base.RecoverJobs(cancellationToken);
    }
}
