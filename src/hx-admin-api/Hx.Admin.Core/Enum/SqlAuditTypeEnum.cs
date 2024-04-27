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

namespace Hx.Admin.Core;
public enum SqlAuditTypeEnum
{
    /// <summary>
    /// 普通日志
    /// </summary>
    [Description("普通日志")]
    Normal = 1,
    /// <summary>
    /// 异常日志
    /// </summary>
    Error = 2,
}
