using Hx.Admin.IService;
using Hx.Admin.Models;
using Hx.Admin.Models.ViewModels.Hub;
using Hx.Admin.Models.ViewModels.Message;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using UAParser;

namespace Hx.Admin.Core;

/// <summary>
/// 在线用户集线器
/// </summary>
//[MapHub("/hubs/onlineUser")]
public class OnlineUserHub : Hub<IOnlineUserHub>
{
    private const string GROUP_ONLINE = "GROUP_ONLINE_"; // 租户分组前缀

    private readonly ISqlSugarRepository<SysOnlineUser> _sysOnlineUerRep;
    private readonly ISysMessageService _sysMessageService;
    private readonly IHubContext<OnlineUserHub, IOnlineUserHub> _onlineUserHubContext;
    private readonly ICache _cache;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public OnlineUserHub(ISqlSugarRepository<SysOnlineUser> sysOnlineUerRep,
        ISysMessageService sysMessageService,
        IHubContext<OnlineUserHub, IOnlineUserHub> onlineUserHubContext,
        ICache cache,
        IHttpContextAccessor httpContextAccessor)
    {
        _sysOnlineUerRep = sysOnlineUerRep;
        _sysMessageService = sysMessageService;
        _onlineUserHubContext = onlineUserHubContext;
        _cache = cache;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// 连接
    /// </summary>
    /// <returns></returns>
    public override async Task OnConnectedAsync()
    {
        var token = _httpContextAccessor.HttpContext?.Request.Query["access_token"];
        //var claims = JwtEncryption.ReadJwtToken(token)?.Claims;
        var claims = _httpContextAccessor.HttpContext?.User.Claims;
        var client = Parser.GetDefault().Parse(_httpContextAccessor.HttpContext?.Request.Headers["User-Agent"]);

        var userId = claims.FirstOrDefault(u => u.Type == ClaimConst.UserId)?.Value;
        var user = new SysOnlineUser
        {
            ConnectionId = Context.ConnectionId,
            UserId = string.IsNullOrWhiteSpace(userId) ? 0 : long.Parse(userId),
            UserName = claims?.FirstOrDefault(u => u.Type == ClaimConst.Account)?.Value,
            RealName = claims.FirstOrDefault(u => u.Type == ClaimConst.RealName)?.Value,
            Time = DateTime.Now,
            Ip = _httpContextAccessor.HttpContext.GetRemoteIpAddressToIPv4(),
            Browser = client.UA.Family + client.UA.Major,
            Os = client.OS.Family + client.OS.Major
        };
        await _sysOnlineUerRep.InsertAsync(user);
        //缓存
        _cache.Set(CacheConst.KeyOnlineUser + user.UserId, user);
        // 以租户Id分组方便区分
        var groupName = $"{GROUP_ONLINE}";
        await _onlineUserHubContext.Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        var userList = await _sysOnlineUerRep.AsQueryable().Filter(null, true)
            .Take(10).ToListAsync();
        await _onlineUserHubContext.Clients.Groups(groupName).OnlineUserList(new OnlineUserList
        {
            RealName = user.RealName,
            Online = true,
            UserList = userList
        });
    }

    /// <summary>
    /// 断开
    /// </summary>
    /// <param name="exception"></param>
    /// <returns></returns>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (string.IsNullOrEmpty(Context.ConnectionId)) return;

        var user = await _sysOnlineUerRep.AsQueryable().Filter(null, true).FirstAsync(u => u.ConnectionId == Context.ConnectionId);
        if (user == null) return;

        await _sysOnlineUerRep.DeleteAsync(u => u.Id == user.Id);
        _cache.Remove(CacheConst.KeyOnlineUser + user.UserId);

        // 通知当前组用户变动
        var userList = await _sysOnlineUerRep.AsQueryable().Filter(null, true)
            .Take(10).ToListAsync();
        await _onlineUserHubContext.Clients.Groups($"{GROUP_ONLINE}").OnlineUserList(new OnlineUserList
        {
            RealName = user.RealName,
            Online = false,
            UserList = userList
        });
    }

    /// <summary>
    /// 强制下线
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task ForceOffline(OnlineUserHubInput input)
    {
        await _onlineUserHubContext.Clients.Client(input.ConnectionId).ForceOffline("强制下线");
    }

    /// <summary>
    /// 前端调用发送方法（发送信息给某个人）
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public async Task ClientsSendMessage(MessageInput message)
    {
        await _sysMessageService.SendUser(message);
    }

    /// <summary>
    /// 前端调用发送方法（发送信息给所有人）
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public async Task ClientsSendMessagetoAll(MessageInput message)
    {
        await _sysMessageService.SendAllUser(message);
    }

    /// <summary>
    /// 前端调用发送方法（发送消息给除了发送人的其他人）
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public async Task ClientsSendMessagetoOther(MessageInput message)
    {
        await _sysMessageService.SendOtherUser(message);
    }

    /// <summary>
    /// 前端调用发送方法（发送消息给某些人）
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public async Task ClientsSendMessagetoUsers(MessageInput message)
    {
        await _sysMessageService.SendUsers(message);
    }
}