/**
 * 系统访问日志表
 * @export
 * @interface SysLogVis
 */
export interface SysLogVis {
	/**
	 * 雪花Id
	 * @type {number}
	 * @memberof SysLogVis
	 */
	id?: number;
	/**
	 * 创建时间
	 * @type {Date}
	 * @memberof SysLogVis
	 */
	createTime?: Date | null;
	/**
	 * 模块名称
	 * @type {string}
	 * @memberof SysLogVis
	 */
	controllerName?: string | null;
	/**
	 * 方法名称
	 * @type {string}
	 * @memberof SysLogVis
	 */
	actionName?: string | null;
	/**
	 * 显示名称
	 * @type {string}
	 * @memberof SysLogVis
	 */
	displayName?: string | null;
	/**
	 * 执行状态
	 * @type {string}
	 * @memberof SysLogVis
	 */
	status?: string | null;
	/**
	 * IP地址
	 * @type {string}
	 * @memberof SysLogVis
	 */
	remoteIp?: string | null;
	/**
	 * 登录地点
	 * @type {string}
	 * @memberof SysLogVis
	 */
	location?: string | null;
	/**
	 * 经度
	 * @type {number}
	 * @memberof SysLogVis
	 */
	longitude?: number | null;
	/**
	 * 维度
	 * @type {number}
	 * @memberof SysLogVis
	 */
	latitude?: number | null;
	/**
	 * 浏览器
	 * @type {string}
	 * @memberof SysLogVis
	 */
	browser?: string | null;
	/**
	 * 操作系统
	 * @type {string}
	 * @memberof SysLogVis
	 */
	os?: string | null;
	/**
	 * 操作用时
	 * @type {number}
	 * @memberof SysLogVis
	 */
	elapsed?: number | null;
	/**
	 * 账号
	 * @type {string}
	 * @memberof SysLogVis
	 */
	account?: string | null;
	/**
	 * 真实姓名
	 * @type {string}
	 * @memberof SysLogVis
	 */
	realName?: string | null;
}
