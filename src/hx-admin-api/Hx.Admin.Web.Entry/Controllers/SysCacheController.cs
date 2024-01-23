// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Admin.IService;
using Hx.Admin.Models.ViewModels.Config;
using Nest;
using System.ComponentModel;
using System.Xml.Linq;

namespace Hx.Admin.Web.Entry.Controllers;

public class SysCacheController : AdminControllerBase
{
    private readonly ICache _cache;
    public SysCacheController(ICache cache)
    {
        _cache = cache;
    }

    /// <summary>
    /// 获取缓存键名集合
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IEnumerable<string> GetKeyList()
    {
        return _cache.GetAllKeys();
    }

    /// <summary>
    /// 删除缓存
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    [HttpPost]
    public void Remove(string key)
    {
        _cache.Remove(key);
    }

    /// <summary>
    /// 根据键名前缀删除缓存
    /// </summary>
    /// <param name="prefixKey">键名前缀</param>
    /// <returns></returns>
    [HttpPost]
    public long RemoveByPrefixKey(string prefixKey)
    {
        return _cache.RemoveByPrefix(prefixKey);
    }

    /// <summary>
    /// 获取缓存值
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    [HttpGet("{key}")]
    public dynamic GetValue(string key)
    {
        return _cache.Get<dynamic>(key);
    }
}