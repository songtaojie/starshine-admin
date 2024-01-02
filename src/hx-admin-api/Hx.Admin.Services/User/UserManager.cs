using Hx.Common.DependencyInjection;
using Microsoft.AspNetCore.Http;

namespace Hx.Admin.Core;

/// <summary>
/// 当前登录用户
/// </summary>
public class UserManager : IScopedDependency
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserManager(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public long UserId
    {
        get
        {
            var uid = _httpContextAccessor?.HttpContext?.User.FindFirst(ClaimConst.UserId)?.Value;
            return string.IsNullOrWhiteSpace(uid) ? 0 : long.Parse(uid);
        }

    }

    public string? Account
    {
        get => _httpContextAccessor?.HttpContext?.User.FindFirst(ClaimConst.Account)?.Value;
    }

    public string? RealName
    {
        get => _httpContextAccessor?.HttpContext?.User.FindFirst(ClaimConst.RealName)?.Value;
    }

    public bool SuperAdmin
    {
        get => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimConst.AccountType)?.Value == ((int)AccountTypeEnum.SuperAdmin).ToString();
    }

    public long OrgId
    {
        get
        {
            var orgId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimConst.OrgId)?.Value;
            return string.IsNullOrWhiteSpace(orgId) ? 0 : long.Parse(orgId);
        }
    }

    public string? OpenId
    {
        get => _httpContextAccessor?.HttpContext?.User.FindFirst(ClaimConst.OpenId)?.Value;
    }

    
}