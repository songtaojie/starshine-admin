﻿// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.AspNetCore.Authorization;

namespace Starshine.Admin.Web.Entry.Controllers;

/// <summary>
/// 授权基础控制器
/// </summary>
[ApiController]
[Route("[controller]/[action]")]
[Authorize(Policy = CommonConst.PermissionPolicy)]
public abstract class AdminControllerBase:ControllerBase
{
}
