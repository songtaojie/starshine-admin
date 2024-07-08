using Starshine.Admin.Models;
using Starshine.Admin.Models.ViewModels.OnlineUser;
using Starshine.DependencyInjection;
using Microsoft.AspNetCore.SignalR;

namespace Starshine.Admin.IService;

/// <summary>
/// 系统在线用户服务
/// </summary>
public interface ISysOnlineUserService : IBaseService<SysOnlineUser>, IScopedDependency
{
    /// <summary>
    /// 获取在线用户分页列表
    /// </summary>
    /// <returns></returns>
    Task<PagedListResult<SysOnlineUser>> GetPage(PageOnlineUserInput input);

    /// <summary>
    /// 强制下线
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task ForceOffline(SysOnlineUser user);

    /// <summary>
    /// 发布站内消息
    /// </summary>
    /// <param name="notice"></param>
    /// <param name="userIds"></param>
    /// <returns></returns>
    Task PublicNotice(SysNotice notice, List<long> userIds);

    /// <summary>
    /// 单用户登录
    /// </summary>
    /// <returns></returns>
    Task SignleLogin(long userId);
}