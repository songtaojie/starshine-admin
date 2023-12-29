// MIT License
//
// Copyright (c) 2021-present zuohuaijun, Daming Co.,Ltd and Contributors
//
// 电话/微信：18020030720 QQ群1：87333204 QQ群2：252381476

using Hx.Admin.Core;
using Hx.Common;
using Hx.Sqlsugar;

namespace Hx.Admin.Repository;
public abstract class BaseRepository<TEntity> where TEntity : EntityBase, new()
{
    protected readonly ISqlSugarRepository<TEntity> _rep;

    public BaseRepository(ISqlSugarRepository<TEntity> rep)
    {
        _rep = rep;
    }

    /// <summary>
    /// 获取实体详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual async Task<TEntity> GetByIdAsync(long id)
    {
        return await _rep.SingleAsync(u => u.Id == id);
    }

    /// <summary>
    /// 获取实体集合
    /// </summary>
    /// <returns></returns>
    public async Task<List<TEntity>> GetList()
    {
        return await _rep.ToListAsync();
    }

    /// <summary>
    /// 获取实体分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public virtual async Task<PagedListResult<TEntity>> GetPage(BasePageInput input)
    {
        return await _rep.AsQueryable().ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加实体
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public virtual async Task<bool> Add(TEntity entity)
    {
        return await _rep.InsertAsync(entity) > 0;
    }

    /// <summary>
    /// 更新实体
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public virtual async Task<bool> Update(TEntity entity)
    {
        return await _rep.UpdateAsync(entity) > 0;
    }

    /// <summary>
    /// 删除实体
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual async Task<bool> Delete(long id)
    {
        return await _rep.DeleteAsync(id) > 0;
    }
}
