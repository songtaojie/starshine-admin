namespace Hx.Admin.Models.ViewModels.File;

public class FileInput : BaseIdParam
{
}

public class PageFileInput : BasePageParam
{
    /// <summary>
    /// 文件名称
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }
}

public class DeleteFileInput : BaseIdParam
{
}