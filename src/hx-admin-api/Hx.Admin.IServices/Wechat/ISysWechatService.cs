using Hx.Admin.Models;
using Hx.Admin.Models.ViewModels.Wechat;
using Hx.Common.DependencyInjection;

namespace Hx.Admin.IService;

/// <summary>
/// 微信公众号服务
/// </summary>
public interface ISysWechatService : IBaseService<SysWechatUser>, IScopedDependency
{
    /// <summary>
    /// 生成网页授权Url
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    string GenAuthUrl(GenAuthUrlInput input);

    /// <summary>
    /// 获取微信用户OpenId
    /// </summary>
    /// <param name="input"></param>
    Task<string> SnsOAuth2(WechatOAuth2Input input);

    /// <summary>
    /// 微信用户登录OpenId
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<dynamic> OpenIdLogin(WechatUserLogin input);

    /// <summary>
    /// 获取配置签名参数(wx.config)
    /// </summary>
    /// <returns></returns>
    Task<dynamic> GenConfigPara(SignatureInput input);
}