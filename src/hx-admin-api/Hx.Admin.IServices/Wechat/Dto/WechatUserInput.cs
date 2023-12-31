namespace Hx.Admin.IService;

public class WechatUserInput : BasePageParam
{
    /// <summary>
    /// 昵称
    /// </summary>
    public string NickName { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    public string PhoneNumber { get; set; }
}

public class DeleteWechatUserInput : BaseIdParam
{
}