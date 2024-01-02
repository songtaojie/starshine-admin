using Hx.Admin.IService;
using Hx.Admin.Models;
using Hx.Admin.Models.ViewModels.Wechat;
using SKIT.FlurlHttpClient.Wechat.Api;
using SKIT.FlurlHttpClient.Wechat.Api.Models;

namespace Hx.Admin.Core.Service;

/// <summary>
/// 微信公众号服务
/// </summary>
public class SysWechatService : BaseService<SysWechatUser>, ISysWechatService
{
    private readonly WechatApiClient _wechatApiClient;

    public SysWechatService(ISqlSugarRepository<SysWechatUser> rep,
        WechatApiClientManager wechatApiHttpClient):base(rep)
    {
        _wechatApiClient = wechatApiHttpClient.CreateWechatClient();
    }

    /// <summary>
    /// 生成网页授权Url
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public string GenAuthUrl(GenAuthUrlInput input)
    {
        return _wechatApiClient.GenerateParameterizedUrlForConnectOAuth2Authorize(input.RedirectUrl, input.Scope, null);
    }

    /// <summary>
    /// 获取微信用户OpenId
    /// </summary>
    /// <param name="input"></param>
    public async Task<string> SnsOAuth2(WechatOAuth2Input input)
    {
        var reqOAuth2 = new SnsOAuth2AccessTokenRequest()
        {
            Code = input.Code,
        };
        var resOAuth2 = await _wechatApiClient.ExecuteSnsOAuth2AccessTokenAsync(reqOAuth2);
        if (resOAuth2.ErrorCode != (int)WechatReturnCodeEnum.请求成功)
            throw new UserFriendlyException($"[{resOAuth2.ErrorCode}]{resOAuth2.ErrorMessage}");

        var wxUser = await FirstOrDefaultAsync(p => p.OpenId == resOAuth2.OpenId);
        if (wxUser == null)
        {
            var reqUserInfo = new SnsUserInfoRequest()
            {
                OpenId = resOAuth2.OpenId,
                AccessToken = resOAuth2.AccessToken,
            };
            var resUserInfo = await _wechatApiClient.ExecuteSnsUserInfoAsync(reqUserInfo);
            wxUser = resUserInfo.Adapt<SysWechatUser>();
            wxUser.Avatar = resUserInfo.HeadImageUrl;
            wxUser.NickName = resUserInfo.Nickname;
            await _rep.InsertAsync(wxUser);
        }
        else
        {
            wxUser.AccessToken = resOAuth2.AccessToken;
            wxUser.RefreshToken = resOAuth2.RefreshToken;
            await _rep.Context.Updateable(wxUser).IgnoreColumns(true).ExecuteCommandAsync();
        }

        return resOAuth2.OpenId;
    }

    /// <summary>
    /// 微信用户登录OpenId
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<dynamic> OpenIdLogin(WechatUserLogin input)
    {
        var wxUser = await FirstOrDefaultAsync(p => p.OpenId == input.OpenId);
        if (wxUser == null)
            throw new UserFriendlyException("微信用户登录OpenId错误");
        return new
        {
            wxUser.Avatar,
            //accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>
            //{
            //    { ClaimConst.UserId, wxUser.Id },
            //    { ClaimConst.OpenId, wxUser.OpenId },
            //    { ClaimConst.NickName, wxUser.NickName },
            //    { ClaimConst.LoginMode, LoginModeEnum.APP },
            //})
        };
    }

    /// <summary>
    /// 获取配置签名参数(wx.config)
    /// </summary>
    /// <returns></returns>
    public async Task<dynamic> GenConfigPara(SignatureInput input)
    {
        var resCgibinToken = await _wechatApiClient.ExecuteCgibinTokenAsync(new CgibinTokenRequest());
        var request = new CgibinTicketGetTicketRequest()
        {
            AccessToken = resCgibinToken.AccessToken
        };
        var response = await _wechatApiClient.ExecuteCgibinTicketGetTicketAsync(request);
        if (!response.IsSuccessful())
            throw new UserFriendlyException(response.ErrorMessage);
        return _wechatApiClient.GenerateParametersForJSSDKConfig(response.Ticket, input.Url);
    }
}