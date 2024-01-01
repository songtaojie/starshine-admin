using Hx.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace Hx.Admin.IService;

/// <summary>
/// 实体操作基服务
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IBaseService<TEntity> where TEntity : EntityBase, new()
{
    #region 查询
    /// <summary>
    /// 根据Id获取模型数据
    /// </summary>
    /// <param name="id">id</param>
    /// <returns>模型数据</returns>
    Task<TEntity> FindAsync(object id);

    /// <summary>
    /// 获取满足指定条件的一条数据
    /// </summary>
    /// <param name="predicate">获取数据的条件lambda</param>
    /// <returns>满足当前条件的一个实体</returns>
    Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool defaultFilter = true);

    #endregion

    #region 新增
    /// <summary>
    /// 插入一条数据
    /// </summary>
    /// <param name="entity">数据实体</param>
    /// <returns></returns>
    Task<bool> InsertAsync(TEntity entity);

    /// <summary>
    /// 插入一条数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<bool> InsertAsync<TModel>([NotNull] TModel model) where TModel : class, new();

    /// <summary>
    /// 插入集合
    /// </summary>
    /// <param name="entityList"></param>
    /// <returns></returns>
    Task<bool> BatchInsertAsync(IEnumerable<TEntity> entityList);
    #endregion

    #region 更新
    Task<bool> UpdateAsync(TEntity entity);

    /// <summary>
    /// 更新实体的部分字段
    /// </summary>
    /// <param name="entity">实体</param>
    /// <param name="fields">要更新的字段的集合</param>
    Task<bool> UpdatePartialAsync(TEntity entity, params string[] fields);
    #endregion

    #region 判断
    Task<bool> ExistAsync(Expression<Func<TEntity, bool>> predicate);
    #endregion

    #region 删除
    /// <summary>
    /// 根据主键删除，不用先查询
    /// </summary>
    /// <param name="id">要删除的主键</param>
    /// <returns></returns>
    Task<bool> DeleteAsync(object id);

    /// <summary>
    /// 根据实体删除数据
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<bool> DeleteAsync(TEntity entity);
    #endregion
}