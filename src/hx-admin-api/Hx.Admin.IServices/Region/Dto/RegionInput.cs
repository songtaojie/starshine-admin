namespace Hx.Admin.IService;

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
    /// <summary>
    /// 名称
    /// </summary>
    [Required(ErrorMessage = "名称不能为空")]
    public override string Name { get; set; }
}

public class UpdateRegionInput : AddRegionInput
{
}

public class DeleteRegionInput : BaseIdParam
{
}