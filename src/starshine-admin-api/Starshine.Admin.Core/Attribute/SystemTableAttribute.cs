﻿namespace Starshine.Admin.Core;

/// <summary>
/// 系统表特性
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class SystemTableAttribute : Attribute
{
}