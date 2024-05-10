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

namespace Hx.Admin.Serilog.Attributes;
/// <summary>
/// 跳过日志监视
/// </summary>
/// <remarks>作用于全局 <see cref="SkipLoggingAttribute"/></remarks>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Parameter, Inherited = true, AllowMultiple = false)]
public sealed class SkipLoggingAttribute : Attribute
{
}
