namespace Hx.Admin.Models.ViewModels.Region;

public class PageRegionInput : BasePageParam
{
    /// <summary>
    /// 父节点Id
    /// </summary>
    public long Pid { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 编码
    /// </summary>
    public string Code { get; set; }
}

public class RegionInput : BaseIdParam
{
}

public class AddRegionInput : SysRegion
{
}

public class UpdateRegionInput : AddRegionInput
{
}

public class DeleteRegionInput : BaseIdParam
{
}