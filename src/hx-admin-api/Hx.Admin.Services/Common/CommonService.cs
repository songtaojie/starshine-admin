using Hx.Admin.IService;
using Hx.Admin.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SqlSugar;
using System.ComponentModel;
using System.Reflection;

namespace Hx.Admin.Core.Service;

public class CommonService : ICommonService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CommonService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// 获取Host
    /// </summary>
    /// <returns></returns>
    public string GetHost()
    {
        var localhost = $"{_httpContextAccessor?.HttpContext?.Request.Scheme}://{_httpContextAccessor?.HttpContext?.Request.Host.Value}";
        return localhost;
    }

    /// <summary>
    /// 获取文件URL
    /// </summary>
    /// <param name="sysFile"></param>
    /// <returns></returns>
    public string GetFileUrl(SysFile sysFile)
    {
        return $"{GetHost()}/{sysFile.FilePath}/{sysFile.Id + sysFile.Suffix}";
    }
}