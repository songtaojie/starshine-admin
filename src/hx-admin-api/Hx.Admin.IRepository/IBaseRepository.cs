using Hx.Common;
using Hx.Sqlsugar;

namespace Hx.Admin.Core;

/// <summary>
/// 实体操作基服务
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IBaseRepository<TEntity> where TEntity : EntityBase, new()
{

    /// <summary>
    /// 获取实体详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<TEntity> GetByIdAsync(long id);

    /// <summary>
    /// 获取实体集合
    /// </summary>
    /// <returns></returns>
    public Task<List<TEntity>> GetList();

    /// <summary>
    /// 获取实体分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Task<PagedListResult<TEntity>> GetPage(BasePageInput input);

    /// <summary>
    /// 增加实体
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public Task<bool> Add(TEntity entity);

    /// <summary>
    /// 更新实体
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public Task<bool> Update(TEntity entity);

    /// <summary>
    /// 删除实体
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<bool> Delete(long id);
}