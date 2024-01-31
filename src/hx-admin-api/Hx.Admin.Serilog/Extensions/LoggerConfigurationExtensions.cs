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

namespace Hx.Admin.Serilog;
public static class LoggerConfigurationExtensions
{
    public static LoggerConfiguration WriteToConsole(this LoggerConfiguration loggerConfiguration)
    {
        //输出普通日志
        loggerConfiguration = loggerConfiguration.WriteTo.Logger(lg =>
            lg.FilterRemoveSqlLog().WriteTo.Console());

        //输出SQL
        loggerConfiguration = loggerConfiguration.WriteTo.Logger(lg =>
            lg.FilterSqlLog().Filter.ByIncludingOnly(Matching.WithProperty<bool>(LogContextConst.SqlOutToConsole, s => s))
                .WriteTo.Console());

        return loggerConfiguration;
    }

    public static LoggerConfiguration WriteToFile(this LoggerConfiguration loggerConfiguration)
    {
        //输出SQL
        loggerConfiguration = loggerConfiguration.WriteTo.Logger(lg =>
            lg.FilterSqlLog().Filter.ByIncludingOnly(Matching.WithProperty<bool>(LogContextConst.SqlOutToFile, s => s))
                .WriteTo.Async(s => s.File(LogContextConst.Combine(LogContextConst.AopSql, @"AopSql.txt"), rollingInterval: RollingInterval.Day,
                    outputTemplate: LogContextConst.FileMessageTemplate, retainedFileCountLimit: 31)));
        //输出普通日志
        loggerConfiguration = loggerConfiguration.WriteTo.Logger(lg =>
            lg.FilterRemoveSqlLog().WriteTo.Async(s => s.File(LogContextConst.Combine(@"Log.txt"), rollingInterval: RollingInterval.Day,
                outputTemplate: LogContextConst.FileMessageTemplate, retainedFileCountLimit: 31)));
        return loggerConfiguration;
    }



    public static LoggerConfiguration FilterSqlLog(this LoggerConfiguration lc)
    {
        lc = lc.Filter.ByIncludingOnly(Matching.WithProperty<string>(LogContextConst.LogSource, s => LogContextConst.AopSql.Equals(s)));
        return lc;
    }

    public static IEnumerable<LogEvent> FilterSqlLog(this IEnumerable<LogEvent> batch)
    {
        return batch.Where(s =>s.FilterSqlLog());
    }

    public static bool FilterSqlLog(this LogEvent logEvent)
    {
        //只记录 Insert、Update、Delete语句
        if (logEvent.WithProperty<string>(LogContextConst.LogSource, q => LogContextConst.AopSql.Equals(q)))
        {
            return logEvent.WithProperty<SugarActionType>(LogContextConst.SugarActionType,
                q => !new[] { SugarActionType.UnKnown, SugarActionType.Query }.Contains(q));
        }
        return false;
    }

    public static LoggerConfiguration FilterRemoveSqlLog(this LoggerConfiguration lc)
    {
        lc = lc.Filter.ByIncludingOnly(WithProperty<string>(LogContextConst.LogSource, s => !LogContextConst.AopSql.Equals(s)));
        return lc;
    }

    public static IEnumerable<LogEvent> FilterRemoveOtherLog(this IEnumerable<LogEvent> batch)
    {
        return batch.Where(s => WithProperty<string>(LogContextConst.LogSource,
            q => !LogContextConst.AopSql.Equals(q))(s));
    }

    public static bool FilterRemoveOtherLog(this LogEvent logEvent)
    {
        return WithProperty<string>(LogContextConst.LogSource,
            q => !LogContextConst.AopSql.Equals(q))(logEvent);
    }

    public static Func<LogEvent, bool> WithProperty<T>(string propertyName, Func<T, bool> predicate)
    {
        //如果不包含属性 也认为是true
        return e =>
        {
            if (!e.Properties.TryGetValue(propertyName, out var propertyValue)) return true;

            return propertyValue is ScalarValue { Value: T value } && predicate(value);
        };
    }

    public static bool WithProperty<T>(this LogEvent e, string key, Func<T, bool> predicate)
    {
        if (!e.Properties.TryGetValue(key, out var propertyValue)) return false;

        return propertyValue is ScalarValue { Value: T value } && predicate(value);
    }
}
