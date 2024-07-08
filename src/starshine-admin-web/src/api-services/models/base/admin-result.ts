/**
 * 全局返回结果
 * @export
 * @interface AdminResult
 */
export interface AdminResult<T> {
    /**
     * 状态码
     * @type {number}
     * @memberof AdminResult
     */
    code?: number;
     /**
     * 状态码
     * @type {any}
     * @memberof AdminResult
     */
     errorCode?: any;
    /**
     * 类型success、warning、error
     * @type {string}
     * @memberof AdminResult
     */
    type?: string | null;
    /**
     * 错误信息
     * @type {string}
     * @memberof AdminResult
     */
    message?: string | null;
    /**
     * 数据
     * @type {string}
     * @memberof AdminResult
     */
    data?: T;
    /**
     * 附加数据
     * @type {any}
     * @memberof AdminResult
     */
    extras?: any | null;
    /**
     * 时间
     * @type {Number}
     * @memberof AdminResult
     */
    time?: Number;
  }
  