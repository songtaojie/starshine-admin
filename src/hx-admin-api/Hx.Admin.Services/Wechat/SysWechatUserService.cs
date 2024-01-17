using Hx.Admin.IService;
using Hx.Admin.Models;
using Hx.Admin.Models.ViewModels.Wechat;

namespace Hx.Admin.Core.Service;

/// <summary>
/// 微信账号服务
/// </summary>
public class SysWechatUserService : BaseService<SysWechatUser>, ISysWechatUserService
{

    public SysWechatUserService(ISqlSugarRepository<SysWechatUser> rep):base(rep)
    {
    }

    /// <summary>
    /// 获取微信用户列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<PagedListResult<PageSysWechatUserOutput>> GetPage(PageWechatUserInput input)
    {
        return await _rep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.NickName), u => u.NickName!.Contains(input.NickName))
            .WhereIF(!string.IsNullOrWhiteSpace(input.PhoneNumber), u => u.Mobile!.Contains(input.PhoneNumber))
            .OrderBy(u => u.CreateTime, OrderByType.Desc)
            .Select<PageSysWechatUserOutput>()
            .ToPagedListAsync(input.Page, input.PageSize);
    }
}