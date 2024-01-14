namespace Hx.Admin.Models.ViewModels.Org;

public class ListOrgInput : BaseIdParam
{
    /// <summary>
    /// 名称
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 编码
    /// </summary>
    public string? Code { get; set; }
}

