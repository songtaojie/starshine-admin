﻿namespace Starshine.Admin.Core;

/// <summary>
/// 自定义规范化结果特性
/// </summary>
[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
public class CustomUnifyResultAttribute : Attribute
{
    public string Name { get; set; }

    public CustomUnifyResultAttribute(string name)
    {
        Name = name;
    }
}