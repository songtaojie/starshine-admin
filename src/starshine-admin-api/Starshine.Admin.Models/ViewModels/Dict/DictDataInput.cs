using Starshine.Admin.Models;
using Starshine.Admin.Models.ViewModels;

namespace Starshine.Admin.Models.ViewModels.Dict;


public class GetDataDictDataInput
{
    /// <summary>
    /// 字典类型Id
    /// </summary>
    public long DictTypeId { get; set; }
}

public class QueryDictDataInput
{
    /// <summary>
    /// 编码
    /// </summary>
    [Required(ErrorMessage = "字典唯一编码不能为空")]
    public string Code { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public StatusEnum? Status { get; set; }
}