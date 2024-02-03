// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Serilog.Context;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Admin.Serilog;
public class MutiLogContext : IDisposable
{
    private readonly Stack<IDisposable> _disposableStack = new Stack<IDisposable>();

    public static MutiLogContext Instance => new MutiLogContext();

    public MutiLogContext AddStock(IDisposable disposable)
    {
        _disposableStack.Push(disposable);
        return this;
    }

    public MutiLogContext PushProperty(string name, object value)
    {
        AddStock(LogContext.PushProperty(name, value));
        return this;
    }


    public MutiLogContext PushSqlsugarProperty(ISqlSugarClient db)
    {
        PushProperty(LogContextConst.LogSource, db.CurrentConnectionConfig.ConfigId);
        PushProperty(LogContextConst.SugarActionType, db.SugarActionType);
        return this;
    }
    public void Dispose()
    {
        while (_disposableStack.Count > 0)
        {
            _disposableStack.Pop().Dispose();
        }
    }
}
