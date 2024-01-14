namespace Hx.Admin.Models.ViewModels.Menu;

public class MenuInput
{
    /// <summary>
    /// 标题
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// 菜单类型（1目录 2菜单 3按钮）
    /// </summary>
    public MenuTypeEnum? Type { get; set; }
}

public class AddMenuInput : SysMenu
{

}

public class UpdateMenuInput : AddMenuInput
{
}

public class DeleteMenuInput : BaseIdParam
{
}