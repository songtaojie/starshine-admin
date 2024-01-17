using Hx.Admin.IService;
using Hx.Admin.Models;
using Hx.Admin.Models.ViewModels.Menu;
using Hx.Admin.Models.ViewModels.User;
using Hx.Sdk.Core.DataEncryption;

namespace Hx.Admin.Core.Service;

/// <summary>
/// 系统用户服务
/// </summary>
public class SysUserService : BaseService<SysUser>, ISysUserService
{
    private readonly UserManager _userManager;
    private readonly ISysOrgService _sysOrgService;
    private readonly ISysUserExtOrgService _sysUserExtOrgService;
    private readonly ISysUserRoleService _sysUserRoleService;
    private readonly ISysConfigService _sysConfigService;

    public SysUserService(UserManager userManager,
        ISqlSugarRepository<SysUser> sysUserRep,
        ISysOrgService sysOrgService,
        ISysUserExtOrgService sysUserExtOrgService,
        ISysUserRoleService sysUserRoleService,
        ISysConfigService sysConfigService):base(sysUserRep)
    {
        _userManager = userManager;
        _sysOrgService = sysOrgService;
        _sysUserExtOrgService = sysUserExtOrgService;
        _sysUserRoleService = sysUserRoleService;
        _sysConfigService = sysConfigService;
    }

    /// <summary>
    /// 获取用户分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<PagedListResult<PageUserOutput>> GetPage(PageUserInput input)
    {
        var orgList = input.OrgId.HasValue && input.OrgId.Value > 0 
                ? await _sysOrgService.GetChildIdListWithSelfById(input.OrgId.Value) 
                : _userManager.SuperAdmin 
                ? null 
                : await _sysOrgService.GetUserOrgIdList(); // 各管理员只能看到自己机构下的用户列表

        return await _rep.AsQueryable()
            .WhereIF(orgList != null, u => orgList.Contains(u.OrgId))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Account), u => u.Account.Contains(input.Account))
            .WhereIF(!string.IsNullOrWhiteSpace(input.RealName), u => u.RealName.Contains(input.RealName))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Phone), u => u.Phone!.Contains(input.Phone))
            .WhereIF(!_userManager.SuperAdmin, u => u.AccountType != AccountTypeEnum.SuperAdmin)
            .OrderBy(u => u.Sort)
            .OrderBy(u => u.CreateTime,OrderByType.Desc)
            .Select<PageUserOutput>()
            .ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddUser(AddUserInput input)
    {
        var isExist = await ExistAsync(u => u.Account == input.Account);
        if (isExist)
            throw new UserFriendlyException("账户已存在");

        var password = await _sysConfigService.GetConfigValue<string>(CommonConst.SysPassword);

        var user = input.Adapt<SysUser>();
        user.Password = CryptogramUtil.Encrypt(password);
        await _rep.InsertAsync(user);
        input.Id = user.Id;
        await UpdateRoleAndExtOrg(input);
        return user.Id;
    }

    /// <summary>
    /// 更新角色和扩展机构
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private async Task UpdateRoleAndExtOrg(AddUserInput input)
    {
        await GrantRole(new UserRoleInput { UserId = input.Id, RoleIdList = input.RoleIdList });

        await _sysUserExtOrgService.UpdateUserExtOrg(input.Id, input.ExtOrgIdList);
    }

    /// <summary>
    /// 更新用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateUser(UpdateUserInput input)
    {
        var isExist = await _rep.AsQueryable().Filter(null, true).AnyAsync(u => u.Account == input.Account && u.Id != input.Id);
        if (isExist)
            throw new UserFriendlyException("账户已存在");

        await _rep.Context.Updateable(input.Adapt<SysUser>()).IgnoreColumns(true)
            .IgnoreColumns(u => new { u.AccountType, u.Password, u.Status }).ExecuteCommandAsync();

        await UpdateRoleAndExtOrg(input);
    }

    /// <summary>
    /// 删除用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task DeleteUser(DeleteUserInput input)
    {
        var user = await FirstOrDefaultAsync(u => u.Id == input.Id);
        if (user == null)
            throw new UserFriendlyException("账户信息已存在");
        if (user.AccountType == AccountTypeEnum.SuperAdmin)
            throw new UserFriendlyException("禁止删除此账号");
        if (user.Id == _userManager.UserId)
            throw new UserFriendlyException("禁止删自己账号");

        await DeleteAsync(user);

        // 删除用户角色
        await _sysUserRoleService.DeleteUserRoleByUserId(input.Id);

        // 删除用户扩展机构
        await _sysUserExtOrgService.DeleteUserExtOrgByUserId(input.Id);
    }

    /// <summary>
    /// 查看用户基本信息
    /// </summary>
    /// <returns></returns>
    public async Task<BaseInfoOutput> GetBaseInfo()
    {
        return await _rep.AsQueryable().Where(u => u.Id == _userManager.UserId)
            .Select<BaseInfoOutput>()
            .FirstAsync();
    }

    /// <summary>
    /// 更新用户基本信息
    /// </summary>
    /// <returns></returns>
    public async Task<bool> UpdateBaseInfo(UpdateBaseInfoInput input)
    {
        var user = await FirstOrDefaultAsync(u => u.Id == input.Id);
        if (user == null) throw new UserFriendlyException("用户信息不存在");
        user = input.Adapt(user);
        return await _rep.Context.Updateable(user)
            .UpdateColumns(u => new {u.RealName,u.NickName,u.Avatar,u.Sex,u.Birthday,u.Phone,u.Email,u.Address,u.Introduction,u.Remark,u.Signature })
            .ExecuteCommandAsync() > 0;
    }

    /// <summary>
    /// 设置用户状态
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<int> SetStatus(UserInput input)
    {
        var user = await FirstOrDefaultAsync(u => u.Id == input.Id);
        if (user.AccountType == AccountTypeEnum.SuperAdmin)
            throw new UserFriendlyException("禁止修改此账号信息");

        if (!Enum.IsDefined(typeof(StatusEnum), input.Status))
            throw new UserFriendlyException("状态不正确");

        user.Status = input.Status;
        return await _rep.Context.Updateable(user).UpdateColumns(u => new { u.Status }).ExecuteCommandAsync();
    }

    /// <summary>
    /// 授权用户角色
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task GrantRole(UserRoleInput input)
    {
        var user = await FirstOrDefaultAsync(u => u.Id == input.UserId);
        if (user.AccountType == AccountTypeEnum.SuperAdmin)
            throw new UserFriendlyException("禁止修改此账号信息");

        await _sysUserRoleService.GrantUserRole(input);
    }

    /// <summary>
    /// 修改用户密码
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<int> ChangePwd(ChangePwdInput input)
    {
        var user = await FirstOrDefaultAsync(u => u.Id == _userManager.UserId);
        if (CryptogramUtil.CryptoType == CryptogramEnum.MD5.ToString())
        {
            if (user.Password != MD5Encryption.Encrypt(input.PasswordOld))
                throw new UserFriendlyException("原密码错误");
        }
        else
        {
            if (CryptogramUtil.Decrypt(user.Password) != input.PasswordOld)
                throw new UserFriendlyException("原密码错误");
        }

        user.Password = CryptogramUtil.Encrypt(input.PasswordNew);
        return await _rep.Context.Updateable(user).UpdateColumns(u => u.Password).ExecuteCommandAsync();
    }

    /// <summary>
    /// 重置用户密码
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<string?> ResetPwd(ResetPwdUserInput input)
    {
        var password = await _sysConfigService.GetConfigValue<string>(CommonConst.SysPassword);

        var user = await FirstOrDefaultAsync(u => u.Id == input.Id);
        user.Password = CryptogramUtil.Encrypt(password);
        await _rep.Context.Updateable(user).UpdateColumns(u => u.Password).ExecuteCommandAsync();
        return password;
    }

    /// <summary>
    /// 获取用户拥有角色集合
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<IEnumerable<long>> GetOwnRoleList(long userId)
    {
        return await _sysUserRoleService.GetUserRoleIdList(userId);
    }

    /// <summary>
    /// 获取用户扩展机构集合
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<IEnumerable<SysUserExtOrg>> GetOwnExtOrgList(long userId)
    {
        return await _sysUserExtOrgService.GetUserExtOrgList(userId);
    }
}