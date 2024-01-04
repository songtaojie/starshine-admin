// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using FreeRedis;
using Hx.Cache;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Hx.Admin.Core;

public sealed class WorkerNode
{
    private readonly ILogger _logger;
    private readonly ICache _cache;
    private readonly CacheSettingsOptions _settings;
    private readonly IRedisClient _redisClient;
    private readonly IServiceProvider _serviceProvider;
    public WorkerNode(ILogger<WorkerNode> logger, 
        ICache cache,
        IOptions<CacheSettingsOptions> options,
        IServiceProvider serviceProvider)
    {
        _cache = cache;
        _logger = logger;
        _settings = options.Value;
        _serviceProvider = serviceProvider;
        _redisClient = _serviceProvider.GetService<IRedisClient>();
    }
    
    internal async Task InitWorkerNodesAsync(string serviceName)
    {
        var cacheKey = GetWorkerIdCacheKey(serviceName);
        if (!_cache.ContainsKey(cacheKey))
        {
            _logger.LogInformation("开始初始化WorkerNodes:{0}", cacheKey);
            double count = 0;
            if (_settings.CacheType == Cache.CacheTypeEnum.Memory)
            {
                SortedList<decimal, string> workerIdList = new SortedList<decimal, string>();
                for (long index = 0; index <= IdGenerater.MaxWorkerId; index++)
                {
                    workerIdList.Add(DateTimeOffset.Now.ToUnixTimeMilliseconds(),index.ToString());
                }
                _cache.Set(cacheKey, workerIdList);
                count = workerIdList.Count;
            }
            else
            {
                if (!_redisClient.SetNx(GetWorkerIdLockKey(serviceName), DateTimeOffset.Now.ToUnixTimeMilliseconds(), 2000))
                {
                    await Task.Delay(300);
                    await InitWorkerNodesAsync(serviceName);
                }

                var set = new List<ZMember>();
                for (long index = 0; index <= IdGenerater.MaxWorkerId; index++)
                {
                    set.Add(new ZMember(index.ToString(), DateTimeOffset.Now.ToUnixTimeMilliseconds()));
                }
                _redisClient.ZAdd(GetWorkerIdLockKey(serviceName), set.ToArray());
                count = set.Count;
            }
            _logger.LogInformation("完成初始化WorkerNodes:{0}:{1}", cacheKey, count);
        }
        else
            _logger.LogInformation("WorkerNodes:{0}已经存在", cacheKey);
    }

    internal long GetWorkerId(string serviceName)
    {
        var cacheKey = GetWorkerIdCacheKey(serviceName);
        if (_cache.ContainsKey(cacheKey))
        {
            if (_settings.CacheType == Cache.CacheTypeEnum.Memory)
            {
                var workerIds = _cache.Get<SortedList<decimal, string>>(cacheKey);
                if (workerIds.Any()) return long.Parse(workerIds.First().Value);
            }
            else
            {
                var workerIds = _redisClient.ZRange(cacheKey, 0, -1);
                if(workerIds.Any()) return long.Parse(workerIds.First());
            }
        }
        return 0;
    }

    internal void RefreshWorkerIdScore(string serviceName, long workerId, double? workerIdScore = null)
    {
        if (workerId < 0 || workerId > IdGenerater.MaxWorkerId)
            throw new Exception(string.Format("worker Id 不能大于 {0} 或者小于 0", IdGenerater.MaxWorkerId));

        var cacheKey = GetWorkerIdCacheKey(serviceName);
        var score = workerIdScore == null 
                ? Convert.ToDecimal(DateTimeOffset.Now.ToUnixTimeMilliseconds()) 
                : Convert.ToDecimal(workerIdScore.Value);
        if (_settings.CacheType == Cache.CacheTypeEnum.Memory)
        {
            var workerIds = _cache.Get<SortedList<decimal, string>>(cacheKey);
            if (workerIds.ContainsKey(score))
            {
                workerIds.Remove(score);
            }

            if (workerIds.ContainsValue(workerId.ToString()))
            {
                var item = workerIds.FirstOrDefault(r => r.Value == workerId.ToString());
                if (workerIds.ContainsKey(item.Key))
                {
                    workerIds.Remove(item.Key);
                }
            }
            workerIds.Add(score, workerId.ToString());
            _cache.Set(cacheKey, workerIds);
        }
        else
        {
            _redisClient.ZAdd(cacheKey, score, workerId.ToString());
        }
           
        _logger.LogDebug("刷新 WorkerNodes:{0}:{1}", workerId, score);
    }

    internal static string GetWorkerIdCacheKey(string serviceName) => $"WorkerId:{serviceName}";
    internal static string GetWorkerIdLockKey(string serviceName) => $"Lock:{serviceName}";
}