﻿namespace Starshine.Admin.Models.ViewModels.Role;

/// <summary>
/// 授权角色机构
/// </summary>
public class RoleOrgInput : BaseIdParam
{
    /// <summary>
    /// 数据范围
    /// </summary>
    public int DataScope { get; set; }

    /// <summary>
    /// 机构Id集合
    /// </summary>
    public List<long> OrgIdList { get; set; }
}