namespace Hx.Admin.Models.ViewModels.User;

public class UserInput : BaseIdParam
{
    /// <summary>
    /// 状态
    /// </summary>
    public StatusEnum Status { get; set; }
}



public class AddUserInput : SysUser
{
    /// <summary>
    /// 角色集合
    /// </summary>
    public List<long> RoleIdList { get; set; }

    /// <summary>
    /// 扩展机构集合
    /// </summary>
    public List<SysUserExtOrg> ExtOrgIdList { get; set; }
}

public class UpdateUserInput : AddUserInput
{
}

public class DeleteUserInput : BaseIdParam
{
    /// <summary>
    /// 机构Id
    /// </summary>
    public long OrgId { get; set; }
}

public class ResetPwdUserInput : BaseIdParam
{
}

public class ChangePwdInput
{
    /// <summary>
    /// 当前密码
    /// </summary>
    [Required(ErrorMessage = "当前密码不能为空")]
    public string PasswordOld { get; set; }

    /// <summary>
    /// 新密码
    /// </summary>
    [Required(ErrorMessage = "新密码不能为空")]
    [StringLength(20, MinimumLength = 5, ErrorMessage = "密码需要大于5个字符")]
    public string PasswordNew { get; set; }
}