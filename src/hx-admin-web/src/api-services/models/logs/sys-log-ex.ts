/**
 * 系统访问日志表
 * @export
 * @interface SysLogEx
 */
export interface SysLogEx {
	/**
	 * 雪花Id
	 * @type {number}
	 * @memberof SysLogEx
	 */
	id?: number;
	/**
	 * 创建时间
	 * @type {Date}
	 * @memberof SysLogEx
	 */
	createTime?: Date | null;
	/**
	 * 更新时间
	 * @type {Date}
	 * @memberof SysLogEx
	 */
	updateTime?: Date | null;
	/**
	 * 模块名称
	 * @type {string}
	 * @memberof SysLogEx
	 */
	controllerName?: string | null;
	/**
	 * 方法名称
	 * @type {string}
	 * @memberof SysLogEx
	 */
	actionName?: string | null;
	/**
	 * 显示名称
	 * @type {string}
	 * @memberof SysLogEx
	 */
	displayTitle?: string | null;
	/**
	 * 执行状态
	 * @type {string}
	 * @memberof SysLogEx
	 */
	status?: string | null;
	/**
	 * IP地址
	 * @type {string}
	 * @memberof SysLogEx
	 */
	remoteIp?: string | null;
	/**
	 * 登录地点
	 * @type {string}
	 * @memberof SysLogEx
	 */
	location?: string | null;
	/**
	 * 经度
	 * @type {number}
	 * @memberof SysLogEx
	 */
	longitude?: number | null;
	/**
	 * 维度
	 * @type {number}
	 * @memberof SysLogEx
	 */
	latitude?: number | null;
	/**
	 * 浏览器
	 * @type {string}
	 * @memberof SysLogEx
	 */
	browser?: string | null;
	/**
	 * 操作系统
	 * @type {string}
	 * @memberof SysLogEx
	 */
	os?: string | null;
	/**
	 * 操作用时
	 * @type {number}
	 * @memberof SysLogEx
	 */
	elapsed?: number | null;
	/**
	 * 日志时间
	 * @type {Date}
	 * @memberof SysLogEx
	 */
	logDateTime?: Date | null;
	/**
	 * 账号
	 * @type {string}
	 * @memberof SysLogEx
	 */
	account?: string | null;
	/**
	 * 真实姓名
	 * @type {string}
	 * @memberof SysLogEx
	 */
	realName?: string | null;
}
