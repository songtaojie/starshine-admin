﻿namespace Starshine.Admin.Models;

/// <summary>
/// 系统角色机构表
/// </summary>
[SugarTable(null, "系统角色机构表")]
public class SysRoleOrg : EntityBase
{
    /// <summary>
    /// 角色Id
    /// </summary>
    [SugarColumn(ColumnDescription = "角色Id")]
    public long RoleId { get; set; }

    /// <summary>
    /// 机构Id
    /// </summary>
    [SugarColumn(ColumnDescription = "机构Id")]
    public long OrgId { get; set; }

    /// <summary>
    /// 机构
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    [Navigate(NavigateType.OneToOne, nameof(OrgId))]
    public SysOrg SysOrg { get; set; }
}