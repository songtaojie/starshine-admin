using Hx.Admin.Models;
using Hx.Admin.Models.ViewModels.Wechat;
using Hx.Common.DependencyInjection;

namespace Hx.Admin.IService;

/// <summary>
/// 微信账号服务
/// </summary>
public interface ISysWechatUserService : IBaseService<SysWechatUser>, IScopedDependency
{

    /// <summary>
    /// 获取微信用户列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PagedListResult<PageSysWechatUserOutput>> GetPage(PageWechatUserInput input);
}