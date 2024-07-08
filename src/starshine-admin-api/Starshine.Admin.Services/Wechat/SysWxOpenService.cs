using Starshine.Admin.IService;
using Starshine.Admin.Models;
using Starshine.Admin.Models.ViewModels.Wechat;
using SKIT.FlurlHttpClient.Wechat.Api;
using SKIT.FlurlHttpClient.Wechat.Api.Models;

namespace Starshine.Admin.Core.Service;

/// <summary>
/// 微信小程序服务
/// </summary>
public class SysWxOpenService : BaseService<SysWechatUser>, ISysWxOpenService
{
    private readonly WechatApiClient _wechatApiClient;

    public SysWxOpenService(ISqlSugarRepository<SysWechatUser> rep,
        WechatApiClientManager wechatApiHttpClient):base(rep)
    {
        _wechatApiClient = wechatApiHttpClient.CreateWxOpenClient();
    }

    /// <summary>
    /// 获取微信用户OpenId
    /// </summary>
    /// <param name="input"></param>
    public async Task<WxOpenIdOutput> GetWxOpenId(JsCode2SessionInput input)
    {
        var reqJsCode2Session = new SnsJsCode2SessionRequest()
        {
            JsCode = input.JsCode,
        };
        var resCode2Session = await _wechatApiClient.ExecuteSnsJsCode2SessionAsync(reqJsCode2Session);
        if (resCode2Session.ErrorCode != (int)WechatReturnCodeEnum.请求成功)
            throw new UserFriendlyException($"[{resCode2Session.ErrorCode}]{resCode2Session.ErrorMessage}");

        var wxUser = await FirstOrDefaultAsync(p => p.OpenId == resCode2Session.OpenId);
        if (wxUser == null)
        {
            wxUser = new SysWechatUser
            {
                OpenId = resCode2Session.OpenId,
                UnionId = resCode2Session.UnionId,
                SessionKey = resCode2Session.SessionKey,
                PlatformType = PlatformTypeEnum.微信小程序
            };
           await InsertAsync(wxUser);
        }
        else
        {
            await _rep.Context.Updateable(wxUser).IgnoreColumns(true).ExecuteCommandAsync();
        }

        return new WxOpenIdOutput
        {
            OpenId = resCode2Session.OpenId
        };
    }

    /// <summary>
    /// 获取微信用户电话号码
    /// </summary>
    /// <param name="input"></param>
    public async Task<WxPhoneOutput> GetWxPhone(WxPhoneInput input)
    {
        var accessToken = await GetCgibinToken();
        var reqUserPhoneNumber = new WxaBusinessGetUserPhoneNumberRequest()
        {
            Code = input.Code,
            AccessToken = accessToken,
        };
        var resUserPhoneNumber = await _wechatApiClient.ExecuteWxaBusinessGetUserPhoneNumberAsync(reqUserPhoneNumber);
        if (resUserPhoneNumber.ErrorCode != (int)WechatReturnCodeEnum.请求成功)
            throw new UserFriendlyException($"[{resUserPhoneNumber.ErrorCode}]{resUserPhoneNumber.ErrorMessage}");

        return new WxPhoneOutput
        {
            PhoneNumber = resUserPhoneNumber.PhoneInfo?.PhoneNumber
        };
    }

    /// <summary>
    /// 微信小程序登录OpenId
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<dynamic> WxOpenIdLogin(WxOpenIdLoginInput input)
    {
        var wxUser = await FirstOrDefaultAsync(p => p.OpenId == input.OpenId);
        if (wxUser == null)
            throw new UserFriendlyException("微信小程序登录失败");
        return new
        {
            wxUser.Avatar,
            //accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>
            //{
            //    { ClaimConst.UserId, wxUser.Id },
            //    { ClaimConst.OpenId, wxUser.OpenId },
            //    { ClaimConst.RealName, wxUser.NickName },
            //    { ClaimConst.LoginMode, LoginModeEnum.APP },
            //})
        };
    }

    /// <summary>
    /// 获取订阅消息模板列表
    /// </summary>
    public async Task<dynamic> GetSubscribeMessageTemplateList()
    {
        var accessToken = await GetCgibinToken();
        var reqTemplate = new WxaApiNewTemplateGetTemplateRequest()
        {
            AccessToken = accessToken
        };
        var resTemplate = await _wechatApiClient.ExecuteWxaApiNewTemplateGetTemplateAsync(reqTemplate);
        if (resTemplate.ErrorCode != (int)WechatReturnCodeEnum.请求成功)
            throw new UserFriendlyException($"[{resTemplate.ErrorCode}]{resTemplate.ErrorMessage}");

        return resTemplate.TemplateList;
    }

    /// <summary>
    /// 发送订阅消息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<dynamic> SendSubscribeMessage(SendSubscribeMessageInput input)
    {
        var accessToken = await GetCgibinToken();
        var reqMessage = new CgibinMessageSubscribeSendRequest()
        {
            AccessToken = accessToken,
            TemplateId = input.TemplateId,
            ToUserOpenId = input.ToUserOpenId,
            Data = input.Data,
            MiniProgramState = input.MiniprogramState,
            Language = input.Language,
            MiniProgramPagePath = input.MiniProgramPagePath
        };
        var resMessage = await _wechatApiClient.ExecuteCgibinMessageSubscribeSendAsync(reqMessage);
        return resMessage;
    }

    /// <summary>
    /// 增加订阅消息模板
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<dynamic> AddSubscribeMessageTemplate(AddSubscribeMessageTemplateInput input)
    {
        var accessToken = await GetCgibinToken();
        var reqMessage = new WxaApiNewTemplateAddTemplateRequest()
        {
            AccessToken = accessToken,
            TemplateTitleId = input.TemplateTitleId,
            KeyworkIdList = input.KeyworkIdList,
            SceneDescription = input.SceneDescription
        };
        var resTemplate = await _wechatApiClient.ExecuteWxaApiNewTemplateAddTemplateAsync(reqMessage);
        return resTemplate;
    }

    /// <summary>
    /// 获取Access_token
    /// </summary>
    private async Task<string> GetCgibinToken()
    {
        var reqCgibinToken = new CgibinTokenRequest();
        var resCgibinToken = await _wechatApiClient.ExecuteCgibinTokenAsync(reqCgibinToken);
        if (resCgibinToken.ErrorCode != (int)WechatReturnCodeEnum.请求成功)
            throw new UserFriendlyException($"[{resCgibinToken.ErrorCode}]{resCgibinToken.ErrorMessage}");
        return resCgibinToken.AccessToken;
    }
}