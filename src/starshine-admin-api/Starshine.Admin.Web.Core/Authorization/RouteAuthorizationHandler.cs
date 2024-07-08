// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Starshine.Admin.IService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Starshine;
using Starshine.Extensions;

namespace Microsoft.AspNetCore.Authorization;
//Infrastructure.OperationAuthorizationRequirement
//Infrastructure.RolesAuthorizationRequirement
/// <summary>
/// 权限授权处理器
/// </summary>
public class RouteAuthorizationHandler : AuthorizationHandler<RouteAuthorizationRequirement>
{
    /// <summary>
    /// 验证方案提供对象
    /// </summary>
    public IAuthenticationSchemeProvider Schemes { get; set; }
    private readonly UserManager _userManager;
    private readonly ISysMenuService _sysMenuService;
    private IEnumerable<string> _globalRoutePath = new List<string> { "/login" };
    /// <summary>
    /// 构造函数注入
    /// </summary>
    /// <param name="schemes"></param>
    /// <param name="userManager"></param>
    /// <param name="sysMenuService"></param>
    public RouteAuthorizationHandler(IAuthenticationSchemeProvider schemes, UserManager userManager, ISysMenuService sysMenuService)
    {
        Schemes = schemes;
        _userManager = userManager;
        _sysMenuService = sysMenuService;
    }

    /// <summary>
    /// 重写异步处理程序
    /// </summary>
    /// <param name="context"></param>
    /// <param name="requirement"></param>
    /// <returns></returns>
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RouteAuthorizationRequirement requirement)
    {
        var httpContext = context.GetCurrentHttpContext();
        int? tokenExpire = null;
        int refreshTokenExpire = 43200;
        if (httpContext != null)
        {
            var sysConfigService = httpContext.RequestServices.GetService<ISysConfigService>();
            tokenExpire = await sysConfigService.GetTokenExpire();
            refreshTokenExpire = await sysConfigService.GetRefreshTokenExpire();
        }
        
        if (JWTEncryption.AutoRefreshToken(context, httpContext, tokenExpire, refreshTokenExpire))
        {
            await AuthorizeHandleAsync(context, requirement);
        }
        else
        {
            context.Fail(); // 授权失败
            if (httpContext == null) return;
            httpContext.SignoutToSwagger();
        }
    }


    private async Task AuthorizeHandleAsync(AuthorizationHandlerContext context, RouteAuthorizationRequirement requirement)
    {
        var httpContext = context.GetCurrentHttpContext() ?? _userManager.HttpContext;
        var endpoint = httpContext.GetEndpoint();
        // 判断action上是否有跳过授权策略的
        var skipAuthorization = endpoint.Metadata.GetMetadata<SkipRouteAuthorizationAttribute>();
        if (skipAuthorization != null)
        {
            context.Succeed(requirement);
            return;
        }
        //判断控制器上是否标记有SkipRouteAuthorizationAttribute
        var actionDescriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
        var hasSkipAuthorization = actionDescriptor.ControllerTypeInfo.HasAttribute<SkipRouteAuthorizationAttribute>();
        if (hasSkipAuthorization)
        {
            context.Succeed(requirement);
            return;
        }

        // 排除超管
        if (_userManager.IsSuperAdmin)
        {
            context.Succeed(requirement);
            return;
        }
        //处理授权策略，判断该用户能否访问该接口
       
        //var userId = _userContext.UserId;
        //var cacheKey = string.Format(CacheKeyConfig.AuthRouterKey, userId);

        // 整体结构类似认证中间件UseAuthentication的逻辑，具体查看开源地址
        // https://github.com/dotnet/aspnetcore/blob/master/src/Security/Authentication/Core/src/AuthenticationMiddleware.cs
        httpContext.Features.Set<IAuthenticationFeature>(new AuthenticationFeature
        {
            OriginalPath = httpContext.Request.Path,
            OriginalPathBase = httpContext.Request.PathBase
        });
        // Give any IAuthenticationRequestHandler schemes a chance to handle the request
        // 主要作用是: 判断当前是否需要进行远程验证，如果是就进行远程验证
        var handlers = httpContext.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
        foreach (var scheme in await Schemes.GetRequestHandlerSchemesAsync())
        {
            if (await handlers.GetHandlerAsync(httpContext, scheme.Name) is IAuthenticationRequestHandler handler && await handler.HandleRequestAsync())
            {
                context.Fail();
                return;
            }
        }
        var routeList = await _sysMenuService.GetOwnRouteList();

        if (routeList == null || !routeList.Any())
        {
            context.Fail();
            return;
        }
        var routePath = httpContext.Request.Headers["Path"].ToString();
        if (_globalRoutePath.Contains(routePath))
        {
            context.Succeed(requirement);
            return;
        }
        //判断请求是否拥有凭据，即有没有登录
        var defaultAuthenticate = await Schemes.GetDefaultAuthenticateSchemeAsync();
        if (defaultAuthenticate != null)
        {
            var result = await httpContext.AuthenticateAsync(defaultAuthenticate.Name);
            //result?.Principal不为空即登录成功
            if (result?.Principal != null)
            {
                //if (!isTestCurrent)
                httpContext.User = result.Principal;
                context.Succeed(requirement);

                //// 获取当前用户的角色信息
                //var isMatch = routeList.Any(m => FixRoute(routePath).Equals(FixRoute(m), StringComparison.OrdinalIgnoreCase));
                //if (isMatch)
                //{
                //    context.Succeed(requirement);
                //}
                //else
                //{
                //    context.Fail();
                //}
            }
        }
    }

    /// <summary>
    /// 修复路由
    /// </summary>
    /// <param name="route"></param>
    /// <returns></returns>
    private static string FixRoute(string route)
    {
        if (string.IsNullOrEmpty(route)) return string.Empty;
        if (route.StartsWith("/")) return route[1..];
        return route;
    }
}
