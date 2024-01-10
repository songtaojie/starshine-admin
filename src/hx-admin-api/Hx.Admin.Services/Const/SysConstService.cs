using Hx.Admin.IService;
using Hx.Cache;
using System.Linq;
using System.Reflection;

namespace Hx.Admin.Core.Service;

/// <summary>
/// 系统常量服务
/// </summary>
public class SysConstService : ISysConstService
{
    private readonly ICache _cache;

    public SysConstService(ICache cache)
    {
        _cache = cache;
    }

    /// <summary>
    /// 获取所有常量列表
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<ConstOutput>> GetList()
    {
        var key = $"{CacheConst.KeyConst}list";
        var constlist = _cache.Get<List<ConstOutput>>(key);
        if (constlist == null)
        {
            var typeList = GetConstAttributeList();
            constlist = typeList.Select(x => new ConstOutput
            {
                Name = x.CustomAttributes.ToList().FirstOrDefault()?.ConstructorArguments.ToList().FirstOrDefault().Value?.ToString() ?? x.Name,
                Code = x.Name,
                Data = GetData(Convert.ToString(x.Name))
            }).ToList();
            _cache.Set(key, constlist);
        }
        return await Task.FromResult(constlist);
    }

    /// <summary>
    /// 根据类名获取常量数据
    /// </summary>
    /// <param name="typeName"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ConstOutput>> GetData(string typeName)
    {
        var key = $"{CacheConst.KeyConst}{typeName.ToUpper()}";
        var typeList = GetConstAttributeList();
        var type = typeList.FirstOrDefault(x => x.Name == typeName);
        if(type == null) return await Task.FromResult(Array.Empty<ConstOutput>());
        var isEnum = type.BaseType!.Name == "Enum";
        var constlist = type.GetFields()?.WhereIF(isEnum, x => x.FieldType.Name == typeName)
            .Select(x => new ConstOutput
            {
                Name = x.Name,
                Code = isEnum ? (int)x.GetValue(BindingFlags.Instance)! : x.GetValue(BindingFlags.Instance)!
            }).ToArray();
        return await Task.FromResult(constlist ?? Array.Empty<ConstOutput>());
    }

    /// <summary>
    /// 获取常量特性类型列表
    /// </summary>
    /// <returns></returns>
    private List<Type> GetConstAttributeList()
    {
        return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
            .Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(ConstAttribute))).ToList();
    }
}