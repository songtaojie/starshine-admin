namespace Starshine.Admin.Models.ViewModels.Hub;

public class OnlineUserList
{
    public string? RealName { get; set; }

    public bool Online { get; set; }

    public List<SysOnlineUser> UserList { get; set; }
}