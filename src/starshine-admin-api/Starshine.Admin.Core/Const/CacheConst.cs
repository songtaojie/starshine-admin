namespace Starshine.Admin.Core;

/// <summary>
/// 缓存相关常量
/// </summary>
public class CacheConst
{
    /// <summary>
    /// 用户缓存
    /// </summary>
    public const string KeyUser = "sys:user:";

    /// <summary>
    /// 菜单缓存
    /// </summary>
    public const string KeyMenu = "sys:menu:";

    /// <summary>
    /// 权限缓存（按钮集合）
    /// </summary>
    public const string KeyPermission = "sys:permission:";

    /// <summary>
    /// 权限缓存（按钮集合）
    /// </summary>
    public const string Key_Route = "sys:route:";

    /// <summary>
    /// 机构Id集合缓存
    /// </summary>
    public const string KeyOrgIdList = "sys:org:";

    /// <summary>
    /// 最大角色数据范围缓存
    /// </summary>
    public const string KeyMaxDataScopeType = "sys:maxDataScopeType:";

    /// <summary>
    /// 验证码缓存
    /// </summary>
    public const string KeyVerCode = "sys:verCode:";

    /// <summary>
    /// 所有缓存关键字集合
    /// </summary>
    public const string KeyAll = "sys:keys";

    /// <summary>
    /// 定时任务缓存
    /// </summary>
    public const string KeyTimer = "sys:timer:";

    /// <summary>
    /// 在线用户缓存
    /// </summary>
    public const string KeyOnlineUser = "sys:onlineuser:";

    /// <summary>
    /// 常量下拉框
    /// </summary>
    public const string KeyConst = "sys:const:";

    /// <summary>
    /// swagger登录缓存
    /// </summary>
    public const string SwaggerLogin = "sys:swaggerLogin:";

    /// <summary>
    /// token
    /// </summary>
    public const string AuthToken_Key = "sys:auth:user_token:{0}";
}