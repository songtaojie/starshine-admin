import { AxiosResponse, AxiosRequestConfig } from 'axios';
// Some imports not used depending on template conditions
// @ts-ignore
import {  BaseAPI} from '../base';
import { AddMenuInput,AdminResult,MenuOutput,SysMenu } from '../models';
import { DeleteMenuInput } from '../models';
import { MenuTypeEnum } from '../models';
import { UpdateMenuInput } from '../models';

/**
 * SysMenuApi - object-oriented interface
 * @export
 * @class SysMenuApi
 * @extends {BaseAPI}
 */
export class SysMenuApi extends BaseAPI {
    /**
     * 
     * @summary 增加菜单
     * @param {AddMenuInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysMenuApi
     */
    public async addSysMenu(body?: AddMenuInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        const api = `/api/sys-menu/add`;
        return this.PostVoid({api,data:body,...options});
    }
    /**
     * 
     * @summary 删除菜单
     * @param {DeleteMenuInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysMenuApi
     */
    public async deleteSysMenu(body?: DeleteMenuInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        const api = `/api/sys-menu/delete`;
        return this.DeleteVoid({api,data:body,...options});
    }
    /**
     * 
     * @summary 获取菜单列表
     * @param {string} [title] 标题
     * @param {MenuTypeEnum} [type] 菜单类型（1目录 2菜单 3按钮）
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysMenuApi
     */
    public async getSysMenuList(title?: string, type?: MenuTypeEnum, options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResult<Array<SysMenu>>>> {
        const api = `/api/sys-menu/list`;
        const params = {
            title,
            type
        }
        return this.GetAdminResult<Array<SysMenu>>({api,params,...options});
    }
    /**
     * 
     * @summary 获取登录菜单树
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysMenuApi
     */
    public async getLoginMenuTree(options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResult<Array<MenuOutput>>>> {
        const api = `/api/sys-menu/loginmenutree`;
        return this.GetAdminResult<Array<MenuOutput>>({api,...options});
    }
    /**
     * 
     * @summary 获取用户拥有按钮权限集合（缓存）
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysMenuApi
     */
    public async getOwnBtnPermList(options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResult<Array<string>>>> {
        const api = `/api/sys-menu/ownbtnpermlist`;
        return this.GetAdminResult<Array<string>>({api,...options});
    }
    /**
     * 
     * @summary 更新菜单
     * @param {UpdateMenuInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysMenuApi
     */
    public async updateSysMenu(body?: UpdateMenuInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        const api = `/api/sys-menu/update`;
        return this.PostVoid({api,data:body,...options});
    }
}
