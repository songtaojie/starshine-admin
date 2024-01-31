// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Admin.Serilog;
public class LogContextConst
{

    public static readonly string BaseLogs = "Logs";

    public const string LogSource = "LogSource";
    public const string AopSql = "AopSql";
    public const string SqlOutToConsole = "OutToConsole";
    public const string SqlOutToFile = "SqlOutToFile";
    public const string OutToDb = "OutToDb";
    public const string SugarActionType = "SugarActionType";

    public static readonly string FileMessageTemplate = "{NewLine}Date：{Timestamp:yyyy-MM-dd HH:mm:ss.fff}{NewLine}LogLevel：{Level}{NewLine}Message：{Message}{NewLine}{Exception}" + new string('-', 100);

    public static string Combine(string path1)
    {
        return Path.Combine(BaseLogs, path1);
    }

    public static string Combine(string path1, string path2)
    {
        return Path.Combine(BaseLogs, path1, path2);
    }

    public static string Combine(string path1, string path2, string path3)
    {
        return Path.Combine(BaseLogs, path1, path2, path3);
    }
}
