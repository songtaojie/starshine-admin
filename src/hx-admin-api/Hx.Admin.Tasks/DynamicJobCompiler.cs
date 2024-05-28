﻿// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Common.DependencyInjection;
using Hx.Common.Extensions;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Admin.Tasks;
[Injection(Pattern = InjectionPatterns.Self)]
public class DynamicJobCompiler : ISingletonDependency
{
    /// <summary>
    /// 编译代码并返回其中实现 IJob 的类型
    /// </summary>
    /// <param name="script">动态编译的作业代码</param>
    /// <returns></returns>
    public Type? BuildJob(string script)
    {
        // 初始化
        NatashaInitializer.Preheating();
        // 动态创建作业
        var builder = new AssemblyCSharpBuilder(this.GetType().GetAssemblyName())
        {
            Domain = DomainManagement.Random()
        };

        builder.Add(script);

        return builder.GetAssembly().GetTypes().FirstOrDefault(u => typeof(IJob).IsAssignableFrom(u));
    }
}
