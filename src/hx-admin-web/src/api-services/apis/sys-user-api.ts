
import { AxiosResponse,AxiosRequestConfig } from 'axios';
import { BaseAPI} from '../base';
import { AddUserInput, AdminResult,SysUserExtOrg,AdminResultPagedListResult,ChangePwdInput,DeleteUserInput,PageUserInput } from '../models';
import { ResetPwdUserInput,SysUser,UpdateUserInput,UserInput,UserRoleInput } from '../models';

/**
 * SysUserApi - object-oriented interface
 * @export
 * @class SysUserApi
 * @extends {BaseAPI}
 */
export class SysUserApi extends BaseAPI {
    /**
     * 
     * @summary 增加用户
     * @param {AddUserInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysUserApi
     */
    public async addUser(body?: AddUserInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResult<void>>> {
        const api = `/api/sysUser/add`;
        return this.PostAdminResult<void>({api,data:body,...options});
    }
    /**
     * 
     * @summary 查看用户基本信息
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysUserApi
     */
    public async getUserBaseInfo(options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResult<SysUser>>> {
        const api = `/api/sysUser/baseInfo`;
        return this.GetAdminResult<SysUser>({api,...options});
    }
    /**
     * 
     * @summary 更新用户基本信息
     * @param {SysUser} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysUserApi
     */
    public async updateUserBaseInfo(data?: SysUser, options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResult<number>>> {
        const api = `/api/sysUser/baseInfo`;
        return this.PostAdminResult<number>({api,data,...options});
    }
    /**
     * 
     * @summary 修改用户密码
     * @param {ChangePwdInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysUserApi
     */
    public async changeUserPwd(data?: ChangePwdInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResult<number>>> {
        const api = `/api/sysUser/changePwd`;
        return this.PostAdminResult<number>({api,data,...options});
    }
    /**
     * 
     * @summary 删除用户
     * @param {DeleteUserInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysUserApi
     */
    public async deleteUser(data?: DeleteUserInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        const api = `/api/sysUser/delete`;
        return this.DeleteVoid({api,data,...options});
    }
    /**
     * 
     * @summary 授权用户角色
     * @param {UserRoleInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysUserApi
     */
    public async grantUserRole(data?: UserRoleInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        const api = `/api/sysUser/grantRole`;
        return this.PostVoid({api,data,...options});
    }
    /**
     * 
     * @summary 获取用户扩展机构集合
     * @param {number} userId 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysUserApi
     */
    public async getUserOwnExtOrgList(userId: number, options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResult<Array<SysUserExtOrg>>>> {
        const api = `/api/sysUser/ownExtOrgList/${encodeURIComponent(String(userId))}`;
        return this.GetAdminResult<Array<SysUserExtOrg>>({api,...options});
    }
    /**
     * 
     * @summary 获取用户拥有角色集合
     * @param {number} userId 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysUserApi
     */
    public async getUserOwnRoleList(userId: number, options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResult<Array<number>>>> {
        const api = `/api/sysUser/ownRoleList/${encodeURIComponent(String(userId))}`;
        return this.GetAdminResult<Array<number>>({api,...options});
    }
    /**
     * 
     * @summary 获取用户分页列表
     * @param {PageUserInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysUserApi
     */
    public async getUserPage(body?: PageUserInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResultPagedListResult<SysUser>>> {
        const api = `/api/sysUser/page`;
        return this.PageAdminResult<SysUser>({api,params:body,...options});
    }
    /**
     * 
     * @summary 重置用户密码
     * @param {ResetPwdUserInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysUserApi
     */
    public async resetUserPwd(body?: ResetPwdUserInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResult<number>>> {
        const api = `/api/sysUser/resetPwd`;
        return this.PostAdminResult<number>({api,data:body,...options});
    }
    /**
     * 
     * @summary 设置用户状态
     * @param {UserInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysUserApi
     */
    public async setUserStatus(body?: UserInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResult<number>>> {
        const api = `/api/sysUser/setStatus`;
        return this.PostAdminResult<number>({api,data:body,...options});
    }
    /**
     * 
     * @summary 更新用户
     * @param {UpdateUserInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysUserApi
     */
    public async apiSysUserUpdatePost(body?: UpdateUserInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        const api = `/api/sysUser/update`;
        return this.PostVoid({api,data:body,...options});
    }
}
