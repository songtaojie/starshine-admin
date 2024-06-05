// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Admin.IService;
using Hx.Admin.Models;
using Hx.Admin.Models.ViewModels.Job;
using Hx.Common.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Admin.IServices;
public interface ISysJobService : IScopedDependency//IBaseService<QrtzJobDetails,int>, IScopedDependency
{
    ///// <summary>
    ///// 获取作业分页列表 ⏰
    ///// </summary>
    ///// <param name="input"></param>
    ///// <returns></returns>
    //Task<PagedListResult<PageJobDetailOutput>> PageJobDetail(PageJobDetailInput input);


    /// <summary>
    /// 添加触发记录
    /// </summary>
    /// <param name="quartzOptions">动态编译的作业代码</param>
    Task AddTriggerRecord(AddTriggerRecordInput addTriggerRecordInput);

    /// <summary>
    /// 初始化DbJob
    /// </summary>
    /// <param name="quartzOptions">动态编译的作业代码</param>
    Task ScanDbJobToQuartz(QuartzOptions quartzOptions);

}
