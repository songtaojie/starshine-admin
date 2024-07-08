import { AxiosResponse, AxiosRequestConfig } from 'axios';
import { BaseAPI } from '../base';
import { AdminResult, AdminResultPagedListResult, SysLogVis } from '../models';
import { PageLogInput, SysLogOp, SysLogDiff, SysLogEx } from '../models';

/**
 * SysLogApi - object-oriented interface
 * @export
 * @class SysLogApi
 * @extends {BaseAPI}
 */
export class SysLogApi extends BaseAPI {
	/**
	 *
	 * @summary 清空访问日志
	 * @param {*} [options] Override http request option.
	 * @throws {RequiredError}
	 * @memberof SysLogApi
	 */
	public async clearVisLog(): Promise<AxiosResponse<AdminResult<Boolean>>> {
		const api = `/api/sys-log/clearvislog`;
		return this.PostAdminResult<Boolean>({ api });
	}
	/**
	 *
	 * @summary 获取访问日志分页列表
	 * @param {PageLogInput} [params]
	 * @param {*} [options] Override http request option.
	 * @throws {RequiredError}
	 * @memberof SysLogApi
	 */
	public async getVisLogPage(params?: PageLogInput): Promise<AxiosResponse<AdminResultPagedListResult<SysLogVis>>> {
		const api = `/api/sys-log/getvislogpage`;
		return this.PageAdminResult<SysLogVis>({ api, params });
	}

	/**
	 *
	 * @summary 清空操作日志
	 * @param {*} [options] Override http request option.
	 * @throws {RequiredError}
	 * @memberof SysLogApi
	 */
	public async clearOpLog(): Promise<AxiosResponse<AdminResult<Boolean>>> {
		const api = `/api/sys-log/clearoplog`;
		return this.PostAdminResult({ api });
	}
	/**
	 *
	 * @summary 导出操作日志
	 * @param {PageLogInput} [body]
	 * @param {*} [options] Override http request option.
	 * @throws {RequiredError}
	 * @memberof SysLogApi
	 */
	public async exportOpLog(params?: PageLogInput, options?: AxiosRequestConfig): Promise<AxiosResponse<AdminResult<any>>> {
		const api = `/api/sys-log/getoplogpage`;
		return this.GetAdminResult<any>({ api, params, ...options });
	}
	/**
	 *
	 * @summary 获取操作日志分页列表
	 * @param {PageLogInput} [params]
	 * @param {*} [options] Override http request option.
	 * @throws {RequiredError}
	 * @memberof SysLogApi
	 */
	public async getOpLogPage(params?: PageLogInput): Promise<AxiosResponse<AdminResultPagedListResult<SysLogOp>>> {
		const api = `/api/sys-log/getoplogpage`;
		return this.PageAdminResult<SysLogOp>({ api, params });
	}

	/**
	 *
	 * @summary 清空差异日志
	 * @param {*} [options] Override http request option.
	 * @throws {RequiredError}
	 * @memberof SysLogApi
	 */
	public async clearDiffLog(): Promise<AxiosResponse<AdminResult<Boolean>>> {
		const api = `/api/sys-log/cleardifflog`;
		return this.PostAdminResult<Boolean>({ api });
	}
	/**
	 *
	 * @summary 获取差异日志分页列表
	 * @param {PageLogInput} [body]
	 * @param {*} [options] Override http request option.
	 * @throws {RequiredError}
	 * @memberof SysLogApi
	 */
	public async getDiffLogPage(params?: PageLogInput): Promise<AxiosResponse<AdminResultPagedListResult<SysLogDiff>>> {
		const api = `/api/sys-log/getdifflogpage`;
		return this.PageAdminResult<SysLogDiff>({ api, params });
	}

	/**
	 *
	 * @summary 获取异常日志分页列表
	 * @param {PageLogInput} [body]
	 * @param {*} [options] Override http request option.
	 * @throws {RequiredError}
	 * @memberof SysLogApi
	 */
	public async getExLogPage(params?: PageLogInput): Promise<AxiosResponse<AdminResultPagedListResult<SysLogEx>>> {
		const api = `/api/sys-log/getexlogpage`;
		return this.PageAdminResult<SysLogEx>({ api, params });
	}

	/**
	 *
	 * @summary 清空异常日志
	 * @param {*} [options] Override http request option.
	 * @throws {RequiredError}
	 * @memberof SysLogApi
	 */
	public async clearExLog(): Promise<AxiosResponse<AdminResult<Boolean>>> {
		const api = `/api/sys-log/clearexlog`;
		return this.PostAdminResult<Boolean>({ api });
	}

	/**
	 *
	 * @summary 导出操作日志
	 * @param {PageLogInput} [body]
	 * @param {*} [options] Override http request option.
	 * @throws {RequiredError}
	 * @memberof SysLogApi
	 */
	public async exportExLog(params?: PageLogInput, options?: AxiosRequestConfig): Promise<AxiosResponse<AdminResult<any>>> {
		const api = `/api/sys-log/exportexLog`;
		return this.GetAdminResult<any>({ api, params, ...options });
	}
}
