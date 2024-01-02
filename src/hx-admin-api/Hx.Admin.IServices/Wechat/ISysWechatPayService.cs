using Hx.Admin.Models;
using Hx.Admin.Models.ViewModels.Wechat;
using Hx.Common.DependencyInjection;

namespace Hx.Admin.IService;

/// <summary>
/// 微信支付服务
/// </summary>
public interface ISysWechatPayService : IBaseService<SysWechatPay>, IScopedDependency
{
    /// <summary>
    /// 生成JSAPI调起支付所需参数
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    dynamic GenerateParametersForJsapiPay(WechatPayParaInput input);

    /// <summary>
    /// 微信支付统一下单获取Id(商户直连)
    /// </summary>
    Task<dynamic> CreatePayTransaction(WechatPayTransactionInput input);

    /// <summary>
    /// 微信支付统一下单获取Id(服务商模式)
    /// </summary>
    Task<dynamic> CreatePayPartnerTransaction(WechatPayTransactionInput input);

    /// <summary>
    /// 获取支付订单详情
    /// </summary>
    /// <param name="tradeId"></param>
    /// <returns></returns>
    Task<SysWechatPay> GetPayInfo(string tradeId);

    /// <summary>
    /// 微信支付成功回调(商户直连)
    /// </summary>
    /// <returns></returns>
    Task<WechatPayOutput?> PayCallBack();

    /// <summary>
    /// 微信支付成功回调(服务商模式)
    /// </summary>
    /// <returns></returns>
    Task PayPartnerCallBack();
}