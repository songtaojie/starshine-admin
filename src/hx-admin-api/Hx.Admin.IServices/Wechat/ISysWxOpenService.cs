using Hx.Admin.Models;
using Hx.Admin.Models.ViewModels.Wechat;
using Hx.Common.DependencyInjection;

namespace Hx.Admin.IService;

/// <summary>
/// 微信小程序服务
/// </summary>
public interface ISysWxOpenService : IBaseService<SysWechatUser>, IScopedDependency
{
    /// <summary>
    /// 获取微信用户OpenId
    /// </summary>
    /// <param name="input"></param>
    Task<WxOpenIdOutput> GetWxOpenId(JsCode2SessionInput input);

    /// <summary>
    /// 获取微信用户电话号码
    /// </summary>
    /// <param name="input"></param>
    Task<WxPhoneOutput> GetWxPhone(WxPhoneInput input);

    /// <summary>
    /// 微信小程序登录OpenId
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<dynamic> WxOpenIdLogin(WxOpenIdLoginInput input);

    /// <summary>
    /// 获取订阅消息模板列表
    /// </summary>
    Task<dynamic> GetSubscribeMessageTemplateList();

    /// <summary>
    /// 发送订阅消息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<dynamic> SendSubscribeMessage(SendSubscribeMessageInput input);

    /// <summary>
    /// 增加订阅消息模板
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<dynamic> AddSubscribeMessageTemplate(AddSubscribeMessageTemplateInput input);

}