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

namespace Hx.Admin.Serilog;
public static class LoggerConfigurationExtensions
{

    public static LoggerConfiguration FilterSqlLog(this LoggerConfiguration lc)
    {
        lc = lc.Filter.ByIncludingOnly(Matching.WithProperty<string>(LogContextConst.AopSqlLogSource, s => LogContextConst.AopSql.Equals(s)));
        return lc;
    }
    public static IEnumerable<LogEvent> FilterSqlLog(this IEnumerable<LogEvent> batch)
    {
        return batch.Where(s => s.FilterSqlLog());
    }
    public static IEnumerable<LogEvent> FilterWriteToDbLog(this IEnumerable<LogEvent> batch)
    {
        return batch.Where(logEvent => logEvent.WithProperty<bool>(LogContextConst.WriteToDb,r=>r));
    }

    public static bool FilterSqlLog(this LogEvent logEvent)
    {
        ////只记录 Insert、Update、Delete语句
        //if (logEvent.Properties.ContainsKey(LogContextConst.AopSqlLogSource))
        //{
        //    return logEvent.WithProperty<SugarActionType>(LogContextConst.SugarActionType,
        //        q => !new[] { SugarActionType.UnKnown, SugarActionType.Query }.Contains(q));
        //}
        return logEvent.Properties.ContainsKey(LogContextConst.AopSqlLogSource);
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
}
