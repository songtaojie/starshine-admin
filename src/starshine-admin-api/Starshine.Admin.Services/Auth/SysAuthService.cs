using Starshine.Admin.IService;
using Starshine.Admin.Models;
using Starshine.Admin.Models.ViewModels.Auth;
using Lazy.Captcha.Core;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Starshine.Caching;
using Starshine.Sqlsugar;
using Starshine.FriendlyException;
using Starshine.DataEncryption;

namespace Starshine.Admin.Core.Service;

/// <summary>
/// 系统登录授权服务
/// </summary>
public class SysAuthService : BaseService<SysUser>, ISysAuthService
{
    private readonly UserManager _userContext;
    private readonly ISysMenuService _sysMenuService;
    private readonly ISysOnlineUserService _sysOnlineUserService;
    private readonly ISysConfigService _sysConfigService;
    private readonly ICache _cache;
    private readonly ICaptcha _captcha;

    public SysAuthService(UserManager userContext,
        ISqlSugarRepository<SysUser> rep,
        ISysMenuService sysMenuService,
        ISysOnlineUserService sysOnlineUserService,
        ISysConfigService sysConfigService,
        ICache cache,
        ICaptcha captcha
        ) :base(rep)
    {
        _userContext = userContext;
        _sysMenuService = sysMenuService;
        _sysOnlineUserService = sysOnlineUserService;
        _sysConfigService = sysConfigService;
        _cache = cache;
        _captcha = captcha;
    }

    /// <summary>
    /// 登录系统
    /// </summary>
    /// <param name="input"></param>
    /// <remarks>用户名/密码：superadmin/123456</remarks>
    /// <returns></returns>
    public async Task<LoginOutput> Login(LoginInput input)
    {
        // 是否开启验证码
        if (await _sysConfigService.GetConfigValue<bool>(CommonConst.SysCaptcha))
        {
            // 判断验证码
            if (!_captcha.Validate(input.CodeId.ToString(), input.Code))
                throw new UserFriendlyException("验证码不正确");
        }

        // 账号是否存在
        var user = await _rep.AsQueryable()
            .Includes(t => t.SysOrg).Filter(null, true)
            .FirstAsync(u => u.Account == input.Account);
        _ = user ?? throw new UserFriendlyException("账号不存在");

        // 账号是否被冻结
        if (user.Status == StatusEnum.Disable)
            throw new UserFriendlyException("账号已禁用");

        // 密码是否正确
        if (CryptogramUtil.CryptoType == CryptogramEnum.MD5.ToString())
        {
            if (user.Password != MD5Encryption.Encrypt(input.Password))
                throw new UserFriendlyException("账号或密码不正确");
        }
        else
        {
            if (CryptogramUtil.Decrypt(user.Password) != input.Password)
                throw new UserFriendlyException("账号或密码不正确");
        }

        // 单用户登录
        await _sysOnlineUserService.SignleLogin(user.Id);

        var tokenExpire = await _sysConfigService.GetTokenExpire();
        var refreshTokenExpire = await _sysConfigService.GetRefreshTokenExpire();

        // 生成Token令牌
        var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>
        {
            { ClaimTypes.NameIdentifier, user.Id },
            { ClaimTypes.WindowsAccountName,user.Account},
            { ClaimTypes.Role, Enum.GetName(user.AccountType) },
            { ClaimTypes.Name, user.RealName },
            { ClaimTypes.GivenName, user.NickName },
            { StarshineClaimTypes.OrgId, user.OrgId },
        }, tokenExpire);
        // 生成刷新Token令牌
        var refreshToken = JWTEncryption.GenerateRefreshToken(accessToken, refreshTokenExpire);

        // 设置响应报文头
        _userContext.HttpContext.SetTokensOfResponseHeaders(accessToken, refreshToken);

        // Swagger Knife4UI-AfterScript登录脚本
        // ke.global.setAllHeader('Authorization', 'Bearer ' + ke.response.headers['access-token']);

        return new LoginOutput
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    /// <summary>
    /// 获取登录账号
    /// </summary>
    /// <returns></returns>
    public async Task<LoginUserOutput> GetUserInfo()
    {
        var userId = _userContext.GetUserId<long>();
        var user = await FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
            throw new UserFriendlyException("账号不存在") { ErrorCode="9000"};

        // 获取机构
        var org = await _rep.Context.Queryable<SysOrg>()
            .Where(u => u.Id == user.OrgId)
            .Select(u => new
            { 
                u.Id,
                u.Name
            })
            .FirstAsync();
        // 获取职位
        var pos = await _rep.Context.Queryable<SysPos>()
            .Where(u => u.Id == user.PosId)
            .Select(u => new
            {
                u.Id,
                u.Name
            })
            .FirstAsync();
        // 获取拥有按钮权限集合
        var buttons = await _sysMenuService.GetOwnBtnPermList();

        return new LoginUserOutput
        {
            Account = user.Account,
            RealName = user.RealName,
            Avatar = user.Avatar,
            Address = user.Address,
            Signature = user.Signature,
            OrgId = user.OrgId,
            OrgName = org?.Name,
            PosName = pos?.Name,
            Buttons = buttons
        };
    }

    /// <summary>
    /// 获取刷新Token
    /// </summary>
    /// <param name="accessToken"></param>
    /// <returns></returns>
    public async Task<string> GetRefreshToken(string accessToken)
    {
        var refreshTokenExpire = await _sysConfigService.GetRefreshTokenExpire();
        return JWTEncryption.GenerateRefreshToken(accessToken, refreshTokenExpire);
    }

    /// <summary>
    /// 退出系统
    /// </summary>
    public void Logout()
    {
        if (!_userContext.IsAuthenticated)
            throw new UserFriendlyException("账号信息不存在");
        _userContext.HttpContext?.SignoutToSwagger();
    }

    /// <summary>
    /// 获取登录配置
    /// </summary>
    /// <returns></returns>
    public async Task<dynamic> GetSystemConfig()
    {
        var secondVerEnabled = await _sysConfigService.GetConfigValue<bool>(CommonConst.SysSecondVer);
        var captchaEnabled = await _sysConfigService.GetConfigValue<bool>(CommonConst.SysCaptcha);
        var watermarkEnabled = await _sysConfigService.GetConfigValue<bool>(CommonConst.SysWatermark);
        return new { SecondVerEnabled = secondVerEnabled, CaptchaEnabled = captchaEnabled, WatermarkEnabled = watermarkEnabled };
    }

    /// <summary>
    /// 获取验证码
    /// </summary>
    /// <returns></returns>
    public dynamic GetCaptcha()
    {
        var codeId = Guid.NewGuid().ToString();
        var captcha = _captcha.Generate(codeId);
        return new { Id = codeId, Img = captcha.Base64 };
    }

}