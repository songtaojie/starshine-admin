using Starshine.Admin.IService;
using Starshine.Admin.Models;
using Starshine.Admin.Models.ViewModels.Notice;

namespace Starshine.Admin.Core.Service;

/// <summary>
/// 系统通知公告服务
/// </summary>
public class SysNoticeService : BaseService<SysNotice>, ISysNoticeService
{
    private readonly UserManager _userManager;
    private readonly ISysOnlineUserService _sysOnlineUserService;

    public SysNoticeService(
        UserManager userManager,
        ISqlSugarRepository<SysNotice> sysNoticeRep,
        ISysOnlineUserService sysOnlineUserService):base(sysNoticeRep)
    {
        _userManager = userManager;
        _sysOnlineUserService = sysOnlineUserService;
    }

    /// <summary>
    /// 获取通知公告分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<PagedListResult<PageNoticeOutput>> GetPage(PageNoticeInput input)
    {
        var userId = _userManager.GetUserId<long>();
        return await _rep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Title), u => u.Title.Contains(input.Title!.Trim()))
            .WhereIF(input.Type.HasValue, u => u.Type == input.Type)
            .WhereIF(!_userManager.IsSuperAdmin, u => u.CreatorId == userId)
            .OrderBy(u => u.PublicTime, OrderByType.Desc)
            .OrderBy(u => u.CreateTime, OrderByType.Desc)
            .Select<PageNoticeOutput>()
            .ToPagedListAsync(input.Page, input.PageSize);
    }
    public override Task<bool> BeforeInsertAsync(SysNotice entity)
    {
        InitNoticeInfo(entity);
        return base.BeforeInsertAsync(entity);
    }

    public override Task<bool> BeforeUpdateAsync(SysNotice entity)
    {
        InitNoticeInfo(entity);
        return base.BeforeUpdateAsync(entity);
    }


    /// <summary>
    /// 删除通知公告
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task DeleteNotice(DeleteNoticeInput input)
    {
        await _rep.DeleteAsync(u => u.Id == input.Id);

        await _rep.Context.Deleteable<SysNoticeUser>().Where(u => u.NoticeId == input.Id).ExecuteCommandAsync();
    }

    /// <summary>
    /// 发布通知公告
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task Public(NoticeInput input)
    {
        // 更新发布状态和时间
        await _rep.Context.Updateable<SysNotice>()
            .SetColumns(u => new SysNotice
            {
                Status = NoticeStatusEnum.PUBLIC,
                PublicTime = DateTime.Now
            })
            .Where(u => u.Id == input.Id)
            .ExecuteCommandAsync();

        var notice = await FirstOrDefaultAsync(u => u.Id == input.Id);

        // 通知到的人(所有账号)
        var userIdList = await _rep.Context.Queryable<SysUser>().Select(u => u.Id).ToListAsync();

        await _rep.Context.Deleteable<SysNoticeUser>().Where(u => u.NoticeId == notice.Id).ExecuteCommandAsync();
        var noticeUserList = userIdList.Select(u => new SysNoticeUser
        {
            NoticeId = notice.Id,
            UserId = u,
        }).ToList();
        await _rep.Context.Insertable(noticeUserList).ExecuteCommandAsync();

        // 广播所有在线账号
        await _sysOnlineUserService.PublicNotice(notice, userIdList);
    }

    /// <summary>
    /// 设置通知公告已读状态
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task SetRead(NoticeInput input)
    {
        var userId = _userManager.GetUserId<long>();
        await _rep.Context.Updateable<SysNoticeUser>().SetColumns(u => new SysNoticeUser
        {
            ReadStatus = NoticeUserStatusEnum.READ,
            ReadTime = DateTime.Now
        }).Where( u => u.NoticeId == input.Id && u.UserId == userId).ExecuteCommandAsync();
    }

    /// <summary>
    /// 获取接收的通知公告
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<PagedListResult<SysNoticeUser>> GetPageReceived(PageNoticeInput input)
    {
        var userId = _userManager.GetUserId<long>();
        return await _rep.Context.Queryable<SysNoticeUser>().Includes(u => u.SysNotice)
            .Where(u => u.UserId == userId)
            .WhereIF(!string.IsNullOrWhiteSpace(input.Title), u => u.SysNotice.Title.Contains(input.Title!.Trim()))
            .WhereIF(input.Type is > 0, u => u.SysNotice.Type == input.Type)
            .OrderBy(u => u.SysNotice.CreateTime, OrderByType.Desc)
            .ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取未读的通知公告
    /// </summary>
    /// <returns></returns>
    public async Task<List<SysNotice>> GetUnReadList()
    {
        var userId = _userManager.GetUserId<long>();
        var noticeUserList = await _rep.Context.Queryable<SysNoticeUser>().Includes(u => u.SysNotice)
            .Where(u => u.UserId == userId && u.ReadStatus == NoticeUserStatusEnum.UNREAD)
            .OrderBy(u => u.SysNotice.CreateTime, OrderByType.Desc).ToListAsync();
        return noticeUserList.Select(t => t.SysNotice).ToList();
    }

    /// <summary>
    /// 初始化通知公告信息
    /// </summary>
    /// <param name="notice"></param>
    private void InitNoticeInfo(SysNotice notice)
    {
        notice.PublicUserId = _userManager.GetUserId<long>();
        notice.PublicUserName = _userManager.UserName;
        notice.PublicOrgId = _userManager.GetOrgId<long>();
    }
}