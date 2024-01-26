import { AxiosResponse } from 'axios';
import {  BaseAPI} from '../base';
import { AddMenuInput,AdminResult,MenuOutput,SysMenu } from '../models';
import { BaseIdInput } from '../models';
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
     * @param {AddMenuInput} [data] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysMenuApi
     */
    public async addSysMenu(data?: AddMenuInput) : Promise<AxiosResponse<void>> {
        const api = `/api/sys-menu/add`;
        return this.PostVoid({api,data});
    }
    /**
     * 
     * @summary 删除菜单
     * @param {BaseIdInput} [data] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysMenuApi
     */
    public async deleteSysMenu(data?: BaseIdInput) : Promise<AxiosResponse<void>> {
        const api = `/api/sys-menu/delete`;
        return this.DeleteVoid({api,data});
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
    public async getSysMenuList(title?: string, type?: MenuTypeEnum) : Promise<AxiosResponse<AdminResult<Array<SysMenu>>>> {
        const api = `/api/sys-menu/getlist`;
        const params = {
            title,
            type
        }
        return this.GetAdminResult<Array<SysMenu>>({api,params});
    }
    /**
     * 
     * @summary 获取登录菜单树
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysMenuApi
     */
    public async getLoginMenuTree() : Promise<AxiosResponse<AdminResult<Array<MenuOutput>>>> {
        const api = `/api/sys-menu/getloginmenutree`;
        return this.GetAdminResult<Array<MenuOutput>>({api});
    }
    /**
     * 
     * @summary 获取用户拥有按钮权限集合（缓存）
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysMenuApi
     */
    public async getOwnBtnPermList() : Promise<AxiosResponse<AdminResult<Array<string>>>> {
        const api = `/api/sys-menu/getownbtnpermlist`;
        return this.GetAdminResult<Array<string>>({api});
    }
    /**
     * 
     * @summary 更新菜单
     * @param {UpdateMenuInput} [data] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof SysMenuApi
     */
    public async updateSysMenu(data?: UpdateMenuInput) : Promise<AxiosResponse<void>> {
        const api = `/api/sys-menu/update`;
        return this.PostVoid({api,data});
    }
}
