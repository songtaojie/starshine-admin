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

namespace Starshine.Admin.Models.ViewModels.User;
/// <summary>
/// 设置用户状态
/// </summary>
public class SetUserStatusInput : BaseIdParam
{
    /// <summary>
    /// 状态
    /// </summary>
    public StatusEnum Status { get; set; }
}
