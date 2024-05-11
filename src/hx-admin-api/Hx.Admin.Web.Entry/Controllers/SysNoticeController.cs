// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Admin.IService;
using Hx.Admin.Models.ViewModels.Notice;
using Hx.Admin.Models;

namespace Hx.Admin.Web.Entry.Controllers;
/// <summary>
/// 通知公告
/// </summary>
public class SysNoticeController : AdminControllerBase
{
    private readonly ISysNoticeService _service;
    /// <summary>
    /// 通知公告
    /// </summary>
    /// <param name="service"></param>
    public SysNoticeController(ISysNoticeService service)
    {
        _service = service;
    }

    /// <summary>
    /// 获取通知公告分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PagedListResult<PageNoticeOutput>> GetPage([FromQuery] PageNoticeInput input)
    {
        return await _service.GetPage(input);
    }

    /// <summary>
    /// 删除通知公告
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task Delete(DeleteNoticeInput input)
    {
        await _service.DeleteNotice(input);
    }

    /// <summary>
    /// 发布通知公告
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task Public(NoticeInput input)
    { 
        await _service.Public(input);
    }

    /// <summary>
    /// 设置通知公告已读状态
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task SetRead(NoticeInput input)
    { 
        await _service.SetRead(input);
    }

    /// <summary>
    /// 获取接收的通知公告
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PagedListResult<SysNoticeUser>> GetReceivedPage([FromQuery]PageNoticeInput input)
    {
        return await _service.GetPageReceived(input);
    }

    /// <summary>
    /// 获取未读的通知公告
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<SysNotice>> GetUnReadList()
    { 
        return await _service.GetUnReadList();
    }
}
