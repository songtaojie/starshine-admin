// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.Extensions.Logging;
using Quartz.Listener;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Admin.Tasks;
public class SampleSchedulerListener : SchedulerListenerSupport
{
    private readonly ILogger<SampleSchedulerListener> logger;

    public SampleSchedulerListener(ILogger<SampleSchedulerListener> logger)
    {
        this.logger = logger;
    }

    public override Task SchedulerStarted(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Observed scheduler start");
        return Task.CompletedTask;
    }
}
