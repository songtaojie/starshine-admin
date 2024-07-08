namespace Starshine.Admin.Models.ViewModels.Pos;

public class PosInput
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

public class AddPosInput : SysPos
{

}

public class UpdatePosInput : AddPosInput
{
}

public class DeletePosInput : BaseIdParam
{
}