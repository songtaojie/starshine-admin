// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Admin.Serilog.Filters;
public class SqlLogFilter : ILogEventFilter
{
    //readonly LogEventLevel _levelFilter;

    //public SqlLogFilter(LogEventLevel levelFilter = LogEventLevel.Information)
    //{
    //    _levelFilter = levelFilter;
    //}

    public bool IsEnabled(LogEvent logEvent)
    {
        return logEvent.WithProperty<string>(LogContextConst.LogSource,
            q => !LogContextConst.AopSql.Equals(q));
    }
}
