namespace Hx.Admin.Models.ViewModels.Org;

public class OrgInput : BaseIdParam
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 编码
    /// </summary>
    public string Code { get; set; }
}

public class AddOrgInput : SysOrg
{
}

public class UpdateOrgInput : AddOrgInput
{
}

public class DeleteOrgInput : BaseIdParam
{
}