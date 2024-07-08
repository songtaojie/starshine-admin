﻿using Starshine.Admin.IService;
using Starshine.Admin.Models;
using Starshine.Admin.Models.ViewModels.OnlineUser;
using Microsoft.AspNetCore.SignalR;

namespace Starshine.Admin.Core.Service;

/// <summary>
/// 系统在线用户服务
/// </summary>
public class SysOnlineUserService : BaseService<SysOnlineUser>, ISysOnlineUserService
{
    private readonly ISysConfigService _sysConfigService;
    private readonly IHubContext<OnlineUserHub, IOnlineUserHub> _onlineUserHubContext;

    public SysOnlineUserService(ISqlSugarRepository<SysOnlineUser> sysOnlineUerRep,
        ISysConfigService sysConfigService,
        IHubContext<OnlineUserHub, IOnlineUserHub> onlineUserHubContext):base(sysOnlineUerRep)
    {
        _sysConfigService = sysConfigService;
        _onlineUserHubContext = onlineUserHubContext;
    }

    /// <summary>
    /// 获取在线用户分页列表
    /// </summary>
    /// <returns></returns>
    public async Task<PagedListResult<SysOnlineUser>> GetPage(PageOnlineUserInput input)
    {
        return await _rep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.UserName), u => u.UserName!.Contains(input.UserName))
            .WhereIF(!string.IsNullOrWhiteSpace(input.RealName), u => u.RealName!.Contains(input.RealName))
            .ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 强制下线
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task ForceOffline(SysOnlineUser user)
    {
        await _onlineUserHubContext.Clients.Client(user.ConnectionId).ForceOffline("强制下线");
        await  DeleteAsync(user);
    }

    /// <summary>
    /// 发布站内消息
    /// </summary>
    /// <param name="notice"></param>
    /// <param name="userIds"></param>
    /// <returns></returns>
    public async Task PublicNotice(SysNotice notice, List<long> userIds)
    {
        var userList = await _rep.GetListAsync(m => userIds.Contains(m.UserId));
        if (!userList.Any()) return;

        foreach (var item in userList)
        {
            await _onlineUserHubContext.Clients.Client(item.ConnectionId).PublicNotice(notice);
        }
    }

    /// <summary>
    /// 单用户登录
    /// </summary>
    /// <returns></returns>
    public async Task SignleLogin(long userId)
    {
        if (await _sysConfigService.GetConfigValue<bool>(CommonConst.SysSingleLogin))
        {
            var user = await FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null) return;

            await ForceOffline(user);
        }
    }
}