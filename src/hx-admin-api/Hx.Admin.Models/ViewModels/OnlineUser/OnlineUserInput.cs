namespace Hx.Admin.Models.ViewModels.OnlineUser;


public class PageOnlineUserInput : BasePageParam
{
    /// <summary>
    /// 账号名称
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 真实姓名
    /// </summary>
    public string RealName { get; set; }
}