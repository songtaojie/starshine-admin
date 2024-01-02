using Hx.Admin.Core;
using Hx.Admin.Models;
using Hx.Admin.Models.ViewModels.Auth;
using Hx.Common.DependencyInjection;
using Lazy.Captcha.Core;
using Microsoft.Extensions.Caching.Memory;

namespace Hx.Admin.IService;

/// <summary>
/// 系统登录授权服务
/// </summary>
public interface ISysAuthService : IBaseService<SysUser>, IScopedDependency
{
    /// <summary>
    /// 登录系统
    /// </summary>
    /// <param name="input"></param>
    /// <remarks>用户名/密码：superadmin/123456</remarks>
    /// <returns></returns>
    public Task<LoginOutput> Login(LoginInput input);

    /// <summary>
    /// 获取登录账号
    /// </summary>
    /// <returns></returns>
    public Task<LoginUserOutput> GetUserInfo();

    /// <summary>
    /// 获取刷新Token
    /// </summary>
    /// <param name="accessToken"></param>
    /// <returns></returns>
    public string GetRefreshToken(string accessToken);

    /// <summary>
    /// 退出系统
    /// </summary>
    public void Logout();

    /// <summary>
    /// 获取登录配置
    /// </summary>
    /// <returns></returns>
    public Task<dynamic> GetLoginConfig();

    /// <summary>
    /// 获取用户配置
    /// </summary>
    /// <returns></returns>
    public Task<dynamic> GetUserConfig();

    /// <summary>
    /// 获取验证码
    /// </summary>
    /// <returns></returns>
    public dynamic GetCaptcha();

    /// <summary>
    /// swagger登录检查
    /// </summary>
    /// <returns></returns>
    public int SwaggerCheckUrl();
}