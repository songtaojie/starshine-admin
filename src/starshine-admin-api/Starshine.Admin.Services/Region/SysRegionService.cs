using AngleSharp;
using AngleSharp.Html.Dom;
using Starshine.Admin.IService;
using Starshine.Admin.Models;
using Starshine.Admin.Models.ViewModels;
using Starshine.Admin.Models.ViewModels.Region;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Starshine.Admin.Core.Service;

/// <summary>
/// 系统行政区域服务
/// </summary>
public class SysRegionService : BaseService<SysRegion>, ISysRegionService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger _logger;
    public SysRegionService(ISqlSugarRepository<SysRegion> sysRegionRep, 
        ILogger<SysRegionService> logger,
        IServiceScopeFactory serviceScopeFactory) : base(sysRegionRep)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    /// <summary>
    /// 获取行政区域分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<PagedListResult<PageRegionOutput>> GetPage(PageRegionInput input)
    {
        return await _rep.AsQueryable()
            .WhereIF(input.Pid > 0, u => u.Pid == input.Pid || u.Id == input.Pid)
            .WhereIF(!string.IsNullOrWhiteSpace(input.Name), u => u.Name.Contains(input.Name))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Code), u => u.Code.Contains(input.Code))
            .Select<PageRegionOutput>()
            .ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取行政区域列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ListRegionOutput>> GetList(BaseIdParam input)
    {
        return await _rep.AsQueryable().Where(u => u.Pid == input.Id).Select<ListRegionOutput>().ToListAsync();
    }

    public override async Task<bool> BeforeInsertAsync(SysRegion entity)
    {
        var isExist = await ExistAsync(u => u.Name == entity.Name);
        if (isExist)
            throw new UserFriendlyException($"已存在名称为【{entity.Name}】的区域");
        isExist = await ExistAsync(u => u.Code == entity.Code);
        if (isExist)
            throw new UserFriendlyException($"已存在编号为【{entity.Code}】的区域");
        return await base.BeforeInsertAsync(entity);
    }
    public override async Task<bool> BeforeUpdateAsync(SysRegion entity)
    {
        if (entity.Pid != 0)
        {
            var pRegion = await FirstOrDefaultAsync(u => u.Id == entity.Pid);
            if (pRegion == null)
                throw new UserFriendlyException("父级区域信息不存在");
        }
        if (entity.Id == entity.Pid)
            throw new UserFriendlyException("当前节点Id不能与父节点Id相同");
        var isExist = await ExistAsync(u => u.Name == entity.Name && u.Id != entity.Id);
        if (isExist)
            throw new UserFriendlyException($"已存在名称为【{entity.Name}】的区域");
        isExist = await ExistAsync(u => u.Code == entity.Code && u.Id != entity.Id);
        if (isExist)
            throw new UserFriendlyException($"已存在编号为【{entity.Code}】的区域");

        return await base.BeforeUpdateAsync(entity);
    }
 
    /// <summary>
    /// 删除行政区域
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task DeleteRegion(BaseIdParam input)
    {
        var regionTreeList = await _rep.AsQueryable().ToChildListAsync(u => u.Pid, input.Id, true);
        var regionIdList = regionTreeList.Select(u => u.Id).ToList();
        await _rep.DeleteAsync(u => regionIdList.Contains(u.Id));
    }

    /// <summary>
    /// 同步行政区域
    /// </summary>
    /// <returns></returns>
    public async Task Sync()
    {
        await _rep.DeleteAsync(u => u.Id > 0);
        
        _ = Task.Run(async () =>
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                // 国家统计局行政区域2022年
                var url = "http://www.stats.gov.cn/sj/tjbz/tjyqhdmhcxhfdm/2022/index.html";
                var context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
                var dom = await context.OpenAsync(url);
                // 省级
                var itemList = dom.QuerySelectorAll("table.provincetable tr.provincetr td a");
               
                var sysRegionRep = scope.ServiceProvider.GetRequiredService<ISqlSugarRepository<SysRegion>>();
                using var db = sysRegionRep.Context.CopyNew();
                foreach (IHtmlAnchorElement item in itemList)
                {
                    var region = new SysRegion
                    {
                        Id = Yitter.IdGenerator.YitIdHelper.NextId(),
                        Pid = 0,
                        Name = item.TextContent,
                        Remark = item.Href,
                        Level = 1,
                    };
                    await db.Insertable(region).ExecuteCommandAsync();
                    // 市级
                    var dom1 = await context.OpenAsync(item.Href);
                    var itemList1 = dom1.QuerySelectorAll("table.citytable tr.citytr td a");
                    for (var i1 = 0; i1 < itemList1.Length; i1 += 2)
                    {
                        var item1 = (IHtmlAnchorElement)itemList1[i1 + 1];
                        var region1 = new SysRegion
                        {
                            Id = Yitter.IdGenerator.YitIdHelper.NextId(),
                            Pid = region.Id,
                            Name = item1.TextContent,
                            Code = itemList1[i1].TextContent,
                            Remark = item1.Href,
                            Level = 2,
                        };
                        await db.Insertable(region1).ExecuteCommandAsync();

                        // 区县级
                        var dom2 = await context.OpenAsync(item1.Href);
                        var itemList2 = dom2.QuerySelectorAll("table.countytable tr.countytr td a");
                        for (var i2 = 0; i2 < itemList2.Length; i2 += 2)
                        {
                            var item2 = (IHtmlAnchorElement)itemList2[i2 + 1];
                            var region2 = new SysRegion
                            {
                                Id = Yitter.IdGenerator.YitIdHelper.NextId(),
                                Pid = region1.Id,
                                Name = item2.TextContent,
                                Code = itemList2[i2].TextContent,
                                Remark = item2.Href,
                                Level = 3,
                            };
                            await db.Insertable(region2).ExecuteCommandAsync();

                            // 街道级
                            var dom3 = await context.OpenAsync(item2.Href);
                            var itemList3 = dom3.QuerySelectorAll("table.towntable tr.towntr td a");
                            for (var i3 = 0; i3 < itemList3.Length; i3 += 2)
                            {
                                var item3 = (IHtmlAnchorElement)itemList3[i3 + 1];
                                var region3 = new SysRegion
                                {
                                    Id = Yitter.IdGenerator.YitIdHelper.NextId(),
                                    Pid = region2.Id,
                                    Name = item3.TextContent,
                                    Code = itemList3[i3].TextContent,
                                    Remark = item3.Href,
                                    Level = 4,
                                };
                                await db.Insertable(region3).ExecuteCommandAsync();

                                // 村级
                                var dom4 = await context.OpenAsync(item3.Href);
                                var itemList4 = dom4.QuerySelectorAll("table.villagetable tr.villagetr td");
                                for (var i4 = 0; i4 < itemList4.Length; i4 += 3)
                                {
                                    await db.Insertable(new SysRegion
                                    {
                                        Id = Yitter.IdGenerator.YitIdHelper.NextId(),
                                        Pid = region3.Id,
                                        Name = itemList4[i4 + 2].TextContent,
                                        Code = itemList4[i4].TextContent,
                                        CityCode = itemList4[i4 + 1].TextContent,
                                        Level = 5,
                                    }).ExecuteCommandAsync();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "同步行政区域失败");
                throw new UserFriendlyException(ex.Message);
            }
        });
        
    }
}