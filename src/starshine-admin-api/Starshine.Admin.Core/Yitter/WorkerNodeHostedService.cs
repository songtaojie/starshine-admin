// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using log4net.Repository.Hierarchy;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Starshine.Admin.Core;

internal sealed class WorkerNodeHostedService : BackgroundService
{
    private readonly ILogger _logger;
    private readonly string _serviceName;
    private readonly WorkerNode _workerNode;
    private readonly int _millisecondsDelay = 1000 * 60;
    private readonly SnowIdOptions _snowIdOptions;
    public WorkerNodeHostedService(ILogger<WorkerNodeHostedService> logger
       , WorkerNode workerNode,
        IOptions<SnowIdOptions> options)
    {
        var webApiAssembly = Assembly.GetEntryAssembly();
        _serviceName = webApiAssembly.GetName().Name.ToLower().Replace(".", "");
        _workerNode = workerNode;
        _logger = logger;
        _snowIdOptions = options.Value;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        IdGenerater.SetWorkerIdBitLength(_snowIdOptions.WorkerIdBitLength);
        IdGenerater.SetSeqBitLength(_snowIdOptions.SeqBitLength);
        await _workerNode.InitWorkerNodesAsync(_serviceName);
        var workerId = _workerNode.GetWorkerId(_serviceName);
        IdGenerater.SetWorkerId((ushort)workerId);

        await base.StartAsync(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);
        _logger.LogInformation("stopping service {0}", _serviceName);

        var subtractionMilliseconds = 0 - (_millisecondsDelay * 1.5);
        var score = DateTimeOffset.Now.AddMilliseconds(subtractionMilliseconds).ToUnixTimeMilliseconds();
        _workerNode.RefreshWorkerIdScore(_serviceName, IdGenerater.CurrentWorkerId, score);

        _logger.LogInformation("stopped service {0}:{1}", _serviceName, score);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _ = Task.Factory.StartNew(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(_millisecondsDelay, stoppingToken);

                    if (stoppingToken.IsCancellationRequested) break;

                    _workerNode.RefreshWorkerIdScore(_serviceName, IdGenerater.CurrentWorkerId);
                }
                catch (Exception ex)
                {
                    var message = $"{nameof(ExecuteAsync)}:{_serviceName}-{IdGenerater.CurrentWorkerId}";
                    _logger.LogError(ex, message);
                    await Task.Delay(_millisecondsDelay / 3, stoppingToken);
                }
            }
        }, stoppingToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);

        await Task.CompletedTask;
    }
}