﻿// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.Admin.Models.ViewModels.Job;
public class UpdateJobDetailInput:AddJobDetailInput
{
    /// <summary>
    /// 主键id
    /// </summary>
    public int Id {  get; set; }
}
