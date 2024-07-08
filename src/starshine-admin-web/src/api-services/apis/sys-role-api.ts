import { AxiosResponse, AxiosRequestConfig } from 'axios';
import { BaseAPI, RequiredError } from '../base';
import { AddRoleInput } from '../models';
import { AdminResult,BaseIdInput,RoleOutput,SysRole,AdminResultPagedListResult } from '../models';
import { PageRoleInput } from '../models';
import { RoleInput } from '../models';
import { RoleMenuInput } from '../models';
import { RoleOrgInput } from '../models';
import { StatusEnum } from '../models';
import { UpdateRoleInput } from '../models';

/**
 * SysRoleApi - object-oriented interface
 * @export
 * @class SysRoleApi
 * @extends {BaseAPI}
 */
export class SysRoleApi extends BaseAPI {
    /**
     * 
     * @summary 增加角色
     * @param {AddRoleInput} [data] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysRoleApi
     */
    public async addSysRole(data?: AddRoleInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        const api = `/api/sys-role/add`;
        return this.PostVoid({api,data,...options});
    }
    /**
     * 
     * @summary 删除角色
     * @param {DeleteRoleInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysRoleApi
     */
    public async deleteSysRole(data?: BaseIdInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        const api = `/api/sys-role/delete`;
        return this.DeleteVoid({api,data,...options});
    }
    /**
     * 
     * @summary 授权角色数据范围
     * @param {RoleOrgInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysRoleApi
     */
    public async grantDataScope(data?: RoleOrgInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        const api = `/api/sys-role/grantDataScope`;
        return this.PostVoid({api,data,...options});
    }
    /**
     * 
     * @summary 授权角色菜单
     * @param {RoleMenuInput} [data] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysRoleApi
     */
    public async grantRoleMenu(data?: RoleMenuInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        const api = `/api/sys-role/grantMenu`;
        return this.PostVoid({api,data,...options});
    }
    /**
     * 
     * @summary 获取角色列表
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysRoleApi
     */
    public async getSysRoleList(options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResult<Array<RoleOutput>>>> {
        const api = `/api/sys-role/getlist`;
        return this.GetAdminResult<Array<RoleOutput>>({api,...options});
    }
    /**
     * 
     * @summary 根据角色Id获取菜单Id集合
     * @param {number} id 主键Id
     * @param {StatusEnum} [status] 状态
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysRoleApi
     */
    public async getRoleOwnMenuList(id: number, status?: StatusEnum, options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResult<Array<number>>>> {
        if (id === null || id === undefined) {
            throw new RequiredError('id','Required parameter id was null or undefined when calling getRoleOwnMenuList.');
        }
        const api = `/api/sys-role/getownmenulist`;
        const params = {id,status};
        return this.GetAdminResult<Array<number>>({api,params,...options});
    }
    /**
     * 
     * @summary 根据角色Id获取机构Id集合
     * @param {number} id 主键Id
     * @param {StatusEnum} [status] 状态
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysRoleApi
     */
    public async getRoleOwnOrgList(id: number, status?: StatusEnum, options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResult<Array<number>>>> {
        if (id === null || id === undefined) {
            throw new RequiredError('id','Required parameter id was null or undefined when calling getRoleOwnOrgList.');
        }
        const api = `/api/sys-role/getownorglist`;
        const params = {id,status};
        return this.GetAdminResult<Array<number>>({api,params,...options});
    }
    /**
     * 
     * @summary 获取角色分页列表
     * @param {PageRoleInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysRoleApi
     */
    public async getSysRolePage(params?: PageRoleInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResultPagedListResult<SysRole>>> {
        const api = `/api/sys-role/getpage`;
        return this.PageAdminResult<SysRole>({api,params,...options});
    }
    /**
     * 
     * @summary 设置角色状态
     * @param {RoleInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysRoleApi
     */
    public async setSysRoleStatus(data?: RoleInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResult<number>>> {
        const api = `/api/sys-role/setStatus`;
        return this.PostAdminResult<number>({api,data,...options});
    }
    /**
     * 
     * @summary 更新角色
     * @param {UpdateRoleInput} [data] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysRoleApi
     */
    public async updateSysRole(data?: UpdateRoleInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        const api = `/api/sys-role/update`;
        return this.PostVoid({api,data,...options});
    }
}
