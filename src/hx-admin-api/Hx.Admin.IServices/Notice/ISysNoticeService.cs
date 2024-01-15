using Hx.Admin.Models;
using Hx.Admin.Models.ViewModels.Notice;
using Hx.Common.DependencyInjection;

namespace Hx.Admin.IService;

/// <summary>
/// 系统通知公告服务
/// </summary>
public interface ISysNoticeService : IBaseService<SysNotice>, IScopedDependency
{
    /// <summary>
    /// 获取通知公告分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PagedListResult<PageNoticeOutput>> GetPage(PageNoticeInput input);

    /// <summary>
    /// 删除通知公告
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task DeleteNotice(DeleteNoticeInput input);

    /// <summary>
    /// 发布通知公告
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task Public(NoticeInput input);

    /// <summary>
    /// 设置通知公告已读状态
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task SetRead(NoticeInput input);

    /// <summary>
    /// 获取接收的通知公告
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PagedListResult<SysNoticeUser>> GetPageReceived(PageNoticeInput input);

    /// <summary>
    /// 获取未读的通知公告
    /// </summary>
    /// <returns></returns>
    Task<List<SysNotice>> GetUnReadList();
}