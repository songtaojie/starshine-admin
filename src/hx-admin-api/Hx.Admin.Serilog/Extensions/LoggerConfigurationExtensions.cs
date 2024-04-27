// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Serilog.Events;
using Serilog.Filters;
using Serilog;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using Hx.Sqlsugar;

namespace Hx.Admin.Serilog;
public static class LoggerConfigurationExtensions
{

    public static IEnumerable<LogEvent> FilterSqlLog(this IEnumerable<LogEvent> batch)
    {
        return batch.Where(s => s.FilterSqlLog());
    }

    public static bool FilterSqlLog(this LogEvent logEvent)
    {
        return logEvent.Properties.ContainsKey(SugarLogScope.Source);
    }

    public static IEnumerable<LogEvent> FilterNotSqlLog(this IEnumerable<LogEvent> batch)
    {
        return  batch.Where(s => !FilterSqlLog(s));
    }


    public static Func<LogEvent, bool> WithProperty<T>(string propertyName, Func<T, bool> predicate)
    {
        return e =>
        {
            if (!e.Properties.ContainsKey(propertyName)) return false;
            if (!e.Properties.TryGetValue(propertyName, out var propertyValue)) return false;

            return propertyValue is ScalarValue { Value: T value } && predicate(value);
        };
    }

    public static bool WithProperty<T>(this LogEvent e, string key, Func<T, bool> predicate)
    {
        if (!e.Properties.ContainsKey(key)) return false;
        if (!e.Properties.TryGetValue(key, out var propertyValue)) return false;

        return propertyValue is ScalarValue { Value: T value } && predicate(value);
    }

    #region LogContextConst扩展 
    public static T? GetPropertyValue<T>(this LogEvent e,string propertyName)
    {
        if (!e.Properties.ContainsKey(propertyName)) return default;
        if (!e.Properties.TryGetValue(propertyName, out var propertyValue)) return default;
        if (propertyValue is ScalarValue { Value: T value })
        {
            return value;
        }
        return default;
    }
    #endregion
}
