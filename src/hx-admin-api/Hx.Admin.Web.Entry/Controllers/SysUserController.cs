// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Admin.IService;
using Hx.Admin.Models;
using Hx.Admin.Models.ViewModels.Menu;
using Hx.Admin.Models.ViewModels.Role;
using Hx.Admin.Models.ViewModels.User;

namespace Hx.Admin.Web.Entry.Controllers;

/// <summary>
/// 系统用户
/// </summary>
public class SysUserController : AdminControllerBase
{
    private readonly ISysUserService _service;
    /// <summary>
    /// 系统用户
    /// </summary>
    /// <param name="service"></param>
    public SysUserController(ISysUserService service)
    {
        _service = service;
    }

    /// <summary>
    /// 获取用户分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PagedListResult<PageUserOutput>> GetPage([FromQuery] PageUserInput input)
    {
        return await _service.GetPage(input);
    }

    /// <summary>
    /// 增加用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> Add(AddUserInput input)
    {
        return await _service.AddUser(input);
    }

    /// <summary>
    /// 更新用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task Update(UpdateUserInput input)
    {
        await _service.UpdateUser(input);
    }

    /// <summary>
    /// 删除用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task Delete(DeleteUserInput input)
    {
        await _service.DeleteAsync(input.Id);
    }

    /// <summary>
    /// 查看用户基本信息
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<BaseInfoOutput> GetBaseInfo()
    {
        return await _service.GetBaseInfo();
    }

    /// <summary>
    /// 查看用户基本信息
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<bool> UpdateBaseInfo(UpdateBaseInfoInput input)
    {
        return await _service.UpdateBaseInfo(input);
    }

    /// <summary>
    ///  设置用户状态
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<int> SetStatus(UserInput input)
    {
        return await _service.SetStatus(input);
    }

    /// <summary>
    ///  授权用户角色
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task GrantRole(UserRoleInput input)
    {
        await _service.GrantRole(input);
    }

    /// <summary>
    ///  修改用户密码
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<int> ChangePwd(ChangePwdInput input)
    {
        return await _service.ChangePwd(input);
    }

    /// <summary>
    ///  重置用户密码
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<string> ResetPwd(ResetPwdUserInput input)
    {
        return await _service.ResetPwd(input);
    }

    /// <summary>
    ///  获取用户拥有角色集合
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("{userId}")]
    public async Task<IEnumerable<long>> GetOwnRoleList(long userId)
    {
        return await _service.GetOwnRoleList(userId);
    }

    /// <summary>
    ///  获取用户扩展机构集合
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("{userId}")]
    public async Task<IEnumerable<SysUserExtOrg>> GetOwnExtOrgList(long userId)
    {
        return await _service.GetOwnExtOrgList(userId);
    }
}