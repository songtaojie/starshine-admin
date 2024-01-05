using Hx.Admin.IService;
using Hx.Admin.Models;
using Hx.Admin.Models.ViewModels.Auth;
using Hx.Sdk.Core.DataEncryption;
using Lazy.Captcha.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace Hx.Admin.Core.Service;

/// <summary>
/// 系统登录授权服务
/// </summary>
public class SysAuthService : BaseService<SysUser>, ISysAuthService
{
    private readonly UserManager _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ISysMenuService _sysMenuService;
    private readonly ISysOnlineUserService _sysOnlineUserService;
    private readonly ISysConfigService _sysConfigService;
    private readonly IMemoryCache _cache;
    private readonly ICaptcha _captcha;

    public SysAuthService(UserManager userManager,
        ISqlSugarRepository<SysUser> rep,
        IHttpContextAccessor httpContextAccessor,
        ISysMenuService sysMenuService,
        ISysOnlineUserService sysOnlineUserService,
        ISysConfigService sysConfigService,
        IMemoryCache cache,
        ICaptcha captcha):base(rep)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
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
        var user = await _rep.AsQueryable().Includes(t => t.SysOrg).Filter(null, true).FirstAsync(u => u.Account.Equals(input.Account));
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
        //var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>
        //{
        //    { ClaimConst.UserId, user.Id },
        //    { ClaimConst.Account, user.Account },
        //    { ClaimConst.RealName, user.RealName },
        //    { ClaimConst.AccountType, user.AccountType },
        //    { ClaimConst.OrgId, user.OrgId },
        //    { ClaimConst.OrgName, user.SysOrg?.Name },
        //}, tokenExpire);
        var accessToken = string.Empty;
        // 生成刷新Token令牌
        var refreshToken = string.Empty; //JWTEncryption.GenerateRefreshToken(accessToken, refreshTokenExpire);

        // 设置响应报文头
        _httpContextAccessor.HttpContext.SetTokensOfResponseHeaders(accessToken, refreshToken);

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
        var user = await FirstOrDefaultAsync(u => u.Id == _userManager.UserId);
        if (user == null)
            throw new UserFriendlyException("账号不存在") {StatusCode = 401 };

        // 获取机构
        var org = await _rep.Context.Queryable<SysOrg>().FirstAsync(u => u.Id == user.OrgId);
        // 获取职位
        var pos = await _rep.Context.Queryable<SysPos>().FirstAsync(u => u.Id == user.PosId);
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
    public string GetRefreshToken(string accessToken)
    {
        var refreshTokenExpire = _sysConfigService.GetRefreshTokenExpire().GetAwaiter().GetResult();
        return string.Empty; // JWTEncryption.GenerateRefreshToken(accessToken, refreshTokenExpire);
    }

    /// <summary>
    /// 退出系统
    /// </summary>
    public void Logout()
    {
        if (string.IsNullOrWhiteSpace(_userManager.Account))
            throw new UserFriendlyException("账号信息不存在");

        _httpContextAccessor.HttpContext.SignoutToSwagger();
    }

    /// <summary>
    /// 获取登录配置
    /// </summary>
    /// <returns></returns>
    public async Task<dynamic> GetLoginConfig()
    {
        var secondVerEnabled = await _sysConfigService.GetConfigValue<bool>(CommonConst.SysSecondVer);
        var captchaEnabled = await _sysConfigService.GetConfigValue<bool>(CommonConst.SysCaptcha);
        return new { SecondVerEnabled = secondVerEnabled, CaptchaEnabled = captchaEnabled };
    }

    /// <summary>
    /// 获取用户配置
    /// </summary>
    /// <returns></returns>
    public async Task<dynamic> GetUserConfig()
    {
        //返回用户和通用配置
        var watermarkEnabled = await _sysConfigService.GetConfigValue<bool>(CommonConst.SysWatermark);
        return new { WatermarkEnabled = watermarkEnabled };
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

    /// <summary>
    /// swagger登录检查
    /// </summary>
    /// <returns></returns>
    public int SwaggerCheckUrl()
    {
        return _cache.Get<bool>(CacheConst.SwaggerLogin) ? 200 : 401;
    }

    ///// <summary>
    ///// swagger登录提交
    ///// </summary>
    ///// <param name="auth"></param>
    ///// <returns></returns>
    //public int SwaggerSubmitUrl(SpecificationAuth auth)
    //{
    //    var userName = App.GetConfig<string>("SpecificationDocumentSettings:LoginInfo:UserName");
    //    var password = App.GetConfig<string>("SpecificationDocumentSettings:LoginInfo:Password");
    //    if (auth.UserName == userName && auth.Password == password)
    //    {
    //        _cache.Set<bool>(CacheConst.SwaggerLogin, true);
    //        return 200;
    //    }
    //    return 401;
    //}
}