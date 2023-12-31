using FluentEmail.Core;
using Hx.Admin.IService;
using Hx.Admin.Models;
using Hx.Admin.Models.ViewModels.Message;
using Hx.Cache;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;

namespace Hx.Admin.Core.Service;

/// <summary>
/// 系统消息发送服务
/// </summary>
public class SysMessageService : ISysMessageService
{
    private readonly ICache _cache;
    private readonly EmailOptions _emailOptions;
    private readonly IFluentEmail _fluentEmail;
    private readonly IHubContext<OnlineUserHub, IOnlineUserHub> _chatHubContext;

    public SysMessageService(ICache cache,
        IOptions<EmailOptions> emailOptions,
        IFluentEmail fluentEmail,
        IHubContext<OnlineUserHub, IOnlineUserHub> chatHubContext)
    {
        _cache = cache;
        _emailOptions = emailOptions.Value;
        _fluentEmail = fluentEmail;
        _chatHubContext = chatHubContext;
    }

    /// <summary>
    /// 发送消息给所有人
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task SendAllUser(MessageInput input)
    {
        await _chatHubContext.Clients.All.ReceiveMessage(input);
    }

    /// <summary>
    /// 发送消息给除了发送人的其他人
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task SendOtherUser(MessageInput input)
    {
        var user = _cache.Get<SysOnlineUser>($"{CacheConst.KeyOnlineUser}{input.UserId}");
        if (user != null)
        {
            await _chatHubContext.Clients.AllExcept(user.ConnectionId).ReceiveMessage(input);
        }
    }

    /// <summary>
    /// 发送消息给某个人
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task SendUser(MessageInput input)
    {
        var user = _cache.Get<SysOnlineUser>($"{CacheConst.KeyOnlineUser}{input.UserId}");
        if (user == null) return;
        await _chatHubContext.Clients.Client(user.ConnectionId).ReceiveMessage(input);
    }

    /// <summary>
    /// 发送消息给某些人
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task SendUsers(MessageInput input)
    {
        var userlist = new List<string>();
        foreach (var userid in input.UserIds)
        {
            var user = _cache.Get<SysOnlineUser>($"{CacheConst.KeyOnlineUser}{userid}");
            if (user != null) userlist.Add(user.ConnectionId);
        }
        await _chatHubContext.Clients.Clients(userlist).ReceiveMessage(input);
    }

    /// <summary>
    /// 发送邮件
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public async Task SendEmail([Required] string message)
    {
        await _fluentEmail.To(_emailOptions.DefaultToEmail).Body(message).SendAsync();
    }
}