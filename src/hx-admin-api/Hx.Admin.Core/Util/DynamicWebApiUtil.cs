// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Admin.Core;
public static class DynamicWebApiUtil
{
    /// <summary>
    /// <see cref="IsWebApiController(Type)"/> 缓存集合
    /// </summary>
    private static readonly ConcurrentDictionary<Type, bool> _isWebApiControllerCached;

    static DynamicWebApiUtil()
    {
        _isWebApiControllerCached = new ConcurrentDictionary<Type, bool>();
    }

    /// <summary>
    /// 是否是Api控制器
    /// </summary>
    /// <param name="type">type</param>
    /// <returns></returns>
    internal static bool IsWebApiController(Type type)
    {
        return _isWebApiControllerCached.GetOrAdd(type, Function);

        static bool Function(Type type)
        {
            if (type.Assembly.GetName().Name.StartsWith("Microsoft.AspNetCore.OData")) return false;

            if (!type.IsPublic || type.IsPrimitive || type.IsValueType || type.IsAbstract || type.IsInterface || type.IsGenericType) return false;

            if (!typeof(Controller).IsAssignableFrom(type) && typeof(ControllerBase).IsAssignableFrom(type)) return true;

            return false;
        }
    }
}
