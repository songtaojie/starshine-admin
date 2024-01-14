using AngleSharp.Dom;
using Hx.Sqlsugar;
using Nest;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using SqlSugar;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Hx.Admin.Core;

/// <summary>
/// 实体操作基服务
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public abstract class BaseService<TEntity> where TEntity : EntityBase, new()
{
    protected readonly ISqlSugarRepository<TEntity> _rep;

    public BaseService(ISqlSugarRepository<TEntity> rep)
    {
        _rep = rep;
    }

    #region 查询
    /// <summary>
    /// 根据Id获取模型数据
    /// </summary>
    /// <param name="id">id</param>
    /// <returns>模型数据</returns>
    public async Task<TEntity> FindAsync(object id)
    {
        return await Task.FromResult(_rep.Single(id));
    }

    /// <summary>
    /// 获取满足指定条件的一条数据
    /// </summary>
    /// <param name="predicate">获取数据的条件lambda</param>
    /// <returns>满足当前条件的一个实体</returns>
    public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool defaultFilter = true)
    {
        return await _rep.SingleAsync(predicate);
    }
    #endregion

    #region 新增
    /// <summary>
    /// 增加实体
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public virtual async Task<bool> BeforeInsertAsync(TEntity entity)
    {
        return await Task.FromResult(true);
    }

    /// <summary>
    /// 增加实体
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual async Task<long> InsertAsync<TModel>([NotNull] TModel model) where TModel : class,new()
    {
        return await InsertAsync(model.Adapt<TEntity>());
    }

    /// <summary>
    /// 增加实体
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public virtual async Task<long> InsertAsync([NotNull] TEntity entity)
    {
        if (await BeforeInsertAsync(entity))
        {
            await _rep.InsertAsync(entity);
            await AfterInsertAsync(entity);
            return entity.Id;
        }
        return 0;  
    }

    /// <summary>
    /// 增加实体
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public virtual async Task<bool> AfterInsertAsync(TEntity entity)
    {
        return await Task.FromResult(true);
    }

    /// <summary>
    /// 插入集合
    /// </summary>
    /// <param name="entityList"></param>
    /// <returns></returns>
    public virtual async Task<bool> BatchInsertAsync(IEnumerable<TEntity> entityList)
    {
        return await _rep.InsertAsync(entityList) > 0;
    }
    #endregion

    #region 更新

    /// <summary>
    /// 更新实体
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual async Task<bool> UpdateAsync<T>(T model)
    {
        return await UpdateAsync(model.Adapt<TEntity>());
    }

    /// <summary>
    /// 增加实体
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public virtual async Task<bool> BeforeUpdateAsync(TEntity entity)
    {
        return await Task.FromResult(true);
    }

    /// <summary>
    /// 更新实体
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public virtual async Task<bool> UpdateAsync(TEntity entity)
    {
        if (await BeforeUpdateAsync(entity))
        {
            return await _rep.UpdateAsync(entity) > 0;
        }
        return false;
    }
    /// <summary>
    /// 更新实体的部分字段
    /// </summary>
    /// <param name="entity">实体</param>
    /// <param name="fields">要更新的字段的集合</param>
    public virtual async Task<bool> UpdatePartialAsync(TEntity entity, params string[] fields)
    {
        return await _rep.Context.Updateable(entity).UpdateColumns(fields).ExecuteCommandAsync() > 0;
    }
    #endregion

    #region 判断
    public virtual async Task<bool> ExistAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _rep.AsQueryable().Where(predicate).AnyAsync();
    }
    #endregion

    #region 删除
    /// <summary>
    /// 删除实体前的验证
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public virtual async Task<bool> BeforeDeleteAsync(long id)
    {
        return await Task.FromResult(true);
    }

    /// <summary>
    /// 增加实体
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public virtual async Task<bool> BeforeDeleteAsync(TEntity entity)
    {
        return await Task.FromResult(true);
    }

    /// <summary>
    /// 删除实体
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual async Task<bool> DeleteAsync(object id)
    {
        if (id is long longId && await BeforeDeleteAsync(longId))
        {
            return await _rep.DeleteAsync(id) > 0;
        }
        return false;
    }
    /// <summary>
    /// 根据实体删除数据
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public virtual async Task<bool> DeleteAsync(TEntity entity)
    {
        if (await BeforeDeleteAsync(entity))
        {
            return await _rep.DeleteAsync(entity) > 0;
        }
        return false;
    }
    #endregion
}