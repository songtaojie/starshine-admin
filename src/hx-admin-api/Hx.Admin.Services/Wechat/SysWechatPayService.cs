using Hx.Admin.IService;
using Hx.Admin.Models;
using Hx.Admin.Models.ViewModels.Wechat;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SKIT.FlurlHttpClient.Wechat.TenpayV3;
using SKIT.FlurlHttpClient.Wechat.TenpayV3.Events;
using SKIT.FlurlHttpClient.Wechat.TenpayV3.Models;
using SKIT.FlurlHttpClient.Wechat.TenpayV3.Settings;
using System.Text;

namespace Hx.Admin.Core.Service;

/// <summary>
/// 微信支付服务
/// </summary>
public class SysWechatPayService : BaseService<SysWechatPay>, ISysWechatPayService
{
    private readonly WechatPayOptions _wechatPayOptions;
    public readonly WechatTenpayClient WechatTenpayClient;
    private readonly PayCallBackOptions _payCallBackOptions;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SysWechatPayService(ISqlSugarRepository<SysWechatPay> rep,
        IOptions<WechatPayOptions> wechatPayOptions,
        IOptions<PayCallBackOptions> payCallBackOptions,
        IWebHostEnvironment webHostEnvironment,
        IHttpContextAccessor httpContextAccessor) :base(rep)
    {
        _wechatPayOptions = wechatPayOptions.Value;
        _webHostEnvironment = webHostEnvironment;
        _httpContextAccessor = httpContextAccessor;
        WechatTenpayClient = CreateTenpayClient();
        _payCallBackOptions = payCallBackOptions.Value;
    }

    /// <summary>
    /// 初始化微信支付客户端
    /// </summary>
    /// <returns></returns>
    private WechatTenpayClient CreateTenpayClient()
    {
        var cerFilePath = _webHostEnvironment.ContentRootPath + _wechatPayOptions.MerchantCertificatePrivateKey;

        var tenpayClientOptions = new WechatTenpayClientOptions()
        {
            MerchantId = _wechatPayOptions.MerchantId,
            MerchantV3Secret = _wechatPayOptions.MerchantV3Secret,
            MerchantCertificateSerialNumber = _wechatPayOptions.MerchantCertificateSerialNumber,
            MerchantCertificatePrivateKey = File.Exists(cerFilePath) ? File.ReadAllText(cerFilePath) : "",
            PlatformCertificateManager = new InMemoryCertificateManager()
        };
        return new WechatTenpayClient(tenpayClientOptions);
    }

    /// <summary>
    /// 生成JSAPI调起支付所需参数
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public dynamic GenerateParametersForJsapiPay(WechatPayParaInput input)
    {
        return WechatTenpayClient.GenerateParametersForJsapiPayRequest(_wechatPayOptions.AppId, input.PrepayId);
    }

    /// <summary>
    /// 微信支付统一下单获取Id(商户直连)
    /// </summary>
    public async Task<dynamic> CreatePayTransaction(WechatPayTransactionInput input)
    {
        var request = new CreatePayTransactionJsapiRequest()
        {
            OutTradeNumber = DateTimeOffset.Now.ToString("yyyyMMddHHmmssfff") + (new Random()).Next(100, 1000), // 订单号
            AppId = _wechatPayOptions.AppId,
            Description = input.Description,
            Attachment = input.Attachment,
            GoodsTag = input.GoodsTag,
            ExpireTime = DateTimeOffset.Now.AddMinutes(10),
            NotifyUrl = _payCallBackOptions.WechatPayUrl,
            Amount = new CreatePayTransactionJsapiRequest.Types.Amount() { Total = input.Total },
            Payer = new CreatePayTransactionJsapiRequest.Types.Payer() { OpenId = input.OpenId }
        };
        var response = await WechatTenpayClient.ExecuteCreatePayTransactionJsapiAsync(request);
        if (!response.IsSuccessful())
            throw new UserFriendlyException(response.ErrorMessage);

        // 保存订单信息
        var wechatPay = new SysWechatPay()
        {
            MerchantId = _wechatPayOptions.MerchantId,
            OutTradeNumber = request.OutTradeNumber,
            Description = input.Description,
            Attachment = input.Attachment,
            GoodsTag = input.GoodsTag,
            Total = input.Total,
            OpenId = input.OpenId
        };
        await InsertAsync(wechatPay);

        return new
        {
            response.PrepayId,
            request.OutTradeNumber
        };
    }

    /// <summary>
    /// 微信支付统一下单获取Id(服务商模式)
    /// </summary>
    public async Task<dynamic> CreatePayPartnerTransaction(WechatPayTransactionInput input)
    {
        var request = new CreatePayPartnerTransactionJsapiRequest()
        {
            OutTradeNumber = DateTimeOffset.Now.ToString("yyyyMMddHHmmssfff") + (new Random()).Next(100, 1000), // 订单号
            AppId = _wechatPayOptions.AppId,
            MerchantId = _wechatPayOptions.MerchantId,
            SubAppId = _wechatPayOptions.AppId,
            SubMerchantId = _wechatPayOptions.MerchantId,
            Description = input.Description,
            Attachment = input.Attachment,
            GoodsTag = input.GoodsTag,
            ExpireTime = DateTimeOffset.Now.AddMinutes(10),
            NotifyUrl = _payCallBackOptions.WechatPayUrl,
            Amount = new CreatePayPartnerTransactionJsapiRequest.Types.Amount() { Total = input.Total },
            Payer = new CreatePayPartnerTransactionJsapiRequest.Types.Payer() { OpenId = input.OpenId }
        };
        var response = await WechatTenpayClient.ExecuteCreatePayPartnerTransactionJsapiAsync(request);
        if (!response.IsSuccessful())
            throw new UserFriendlyException(response.ErrorMessage);

        // 保存订单信息
        var wechatPay = new SysWechatPay()
        {
            AppId = _wechatPayOptions.AppId,
            MerchantId = _wechatPayOptions.MerchantId,
            SubAppId = _wechatPayOptions.AppId,
            SubMerchantId = _wechatPayOptions.MerchantId,
            OutTradeNumber = request.OutTradeNumber,
            Description = input.Description,
            Attachment = input.Attachment,
            GoodsTag = input.GoodsTag,
            Total = input.Total,
            OpenId = input.OpenId
        };
        await InsertAsync(wechatPay);

        return new
        {
            response.PrepayId,
            request.OutTradeNumber
        };
    }

    /// <summary>
    /// 获取支付订单详情
    /// </summary>
    /// <param name="tradeId"></param>
    /// <returns></returns>
    public async Task<SysWechatPay> GetPayInfo(string tradeId)
    {
        return await FirstOrDefaultAsync(u => u.OutTradeNumber == tradeId);
    }

    /// <summary>
    /// 微信支付成功回调(商户直连)
    /// </summary>
    /// <returns></returns>
    public async Task<WechatPayOutput?> PayCallBack()
    {
        if (_httpContextAccessor == null || _httpContextAccessor.HttpContext == null) return null;
        using var ms = new MemoryStream();
        await _httpContextAccessor.HttpContext.Request.Body.CopyToAsync(ms);
        var b = ms.ToArray();
        var callbackJson = Encoding.UTF8.GetString(b);

        var callbackModel = WechatTenpayClient.DeserializeEvent(callbackJson);
        if ("TRANSACTION.SUCCESS".Equals(callbackModel.EventType))
        {
            var callbackResource = WechatTenpayClient.DecryptEventResource<TransactionResource>(callbackModel);

            // 修改订单支付状态
            var wechatPay = await FirstOrDefaultAsync(u => u.OutTradeNumber == callbackResource.OutTradeNumber
                && u.MerchantId == callbackResource.MerchantId);
            if (wechatPay == null) return null;
            //wechatPay.OpenId = callbackResource.Payer.OpenId; // 支付者标识
            //wechatPay.MerchantId = callbackResource.MerchantId; // 微信商户号
            //wechatPay.OutTradeNumber = callbackResource.OutTradeNumber; // 商户订单号
            wechatPay.TransactionId = callbackResource.TransactionId; // 支付订单号
            wechatPay.TradeType = callbackResource.TradeType; // 交易类型
            wechatPay.TradeState = callbackResource.TradeState; // 交易状态
            wechatPay.TradeStateDescription = callbackResource.TradeStateDescription; // 交易状态描述
            wechatPay.BankType = callbackResource.BankType; // 付款银行类型
            wechatPay.Total = callbackResource.Amount.Total; // 订单总金额
            wechatPay.PayerTotal = callbackResource.Amount.PayerTotal ?? 0; // 用户支付金额
            wechatPay.SuccessTime = callbackResource.SuccessTime; // 支付完成时间

            await _rep.Context.Updateable(wechatPay).IgnoreColumns(true).ExecuteCommandAsync();

            return new WechatPayOutput()
            {
                Total = wechatPay.Total,
                Attachment = wechatPay.Attachment,
                GoodsTag = wechatPay.GoodsTag
            };
        }

        return null;
    }

    /// <summary>
    /// 微信支付成功回调(服务商模式)
    /// </summary>
    /// <returns></returns>
    public async Task PayPartnerCallBack()
    {
        if (_httpContextAccessor == null || _httpContextAccessor.HttpContext == null) return;
        using var ms = new MemoryStream();
        await _httpContextAccessor.HttpContext.Request.Body.CopyToAsync(ms);
        var b = ms.ToArray();
        var callbackJson = Encoding.UTF8.GetString(b);

        var callbackModel = WechatTenpayClient.DeserializeEvent(callbackJson);
        if ("TRANSACTION.SUCCESS".Equals(callbackModel.EventType))
        {
            var callbackResource = WechatTenpayClient.DecryptEventResource<PartnerTransactionResource>(callbackModel);

            // 修改订单支付状态
            var wechatPay = await FirstOrDefaultAsync(u => u.OutTradeNumber == callbackResource.OutTradeNumber
                && u.MerchantId == callbackResource.MerchantId);
            if (wechatPay == null) return;
            //wechatPay.OpenId = callbackResource.Payer.OpenId; // 支付者标识
            //wechatPay.MerchantId = callbackResource.MerchantId; // 微信商户号
            //wechatPay.OutTradeNumber = callbackResource.OutTradeNumber; // 商户订单号
            wechatPay.TransactionId = callbackResource.TransactionId; // 支付订单号
            wechatPay.TradeType = callbackResource.TradeType; // 交易类型
            wechatPay.TradeState = callbackResource.TradeState; // 交易状态
            wechatPay.TradeStateDescription = callbackResource.TradeStateDescription; // 交易状态描述
            wechatPay.BankType = callbackResource.BankType; // 付款银行类型
            wechatPay.Total = callbackResource.Amount.Total; // 订单总金额
            wechatPay.PayerTotal = callbackResource.Amount.PayerTotal ?? 0; // 用户支付金额
            wechatPay.SuccessTime = callbackResource.SuccessTime; // 支付完成时间

            await _rep.Context.Updateable(wechatPay).IgnoreColumns(true).ExecuteCommandAsync();
        }
    }
}