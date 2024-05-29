// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Admin.Models.ViewModels.Job;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Admin.IServices;
public interface ISysJobService
{
    /// <summary>
    /// 获取作业分页列表 ⏰
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PagedListResult<PageJobDetailOutput>> PageJobDetail(PageJobDetailInput input);
}
