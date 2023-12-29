using AspNetCoreRateLimit;

namespace Hx.Admin.Core;

/// <summary>
/// IP限流配置选项
/// </summary>
public sealed class IpRateLimitingOptions : IpRateLimitOptions
{
}

/// <summary>
/// IP限流策略配置选项
/// </summary>
public sealed class IpRateLimitPoliciesOptions : IpRateLimitPolicies
{
}

/// <summary>
/// 客户端限流配置选项
/// </summary>
public sealed class ClientRateLimitingOptions : ClientRateLimitOptions
{
}

/// <summary>
/// 客户端限流策略配置选项
/// </summary>
public sealed class ClientRateLimitPoliciesOptions : ClientRateLimitPolicies
{
}