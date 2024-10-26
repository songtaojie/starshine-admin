/* tslint:disable */
/* eslint-disable */

/**
 * 
 * @export
 * @interface PageOnlineUserInput
 */
export interface PageOnlineUserInput {
    /**
     * 当前页码
     * @type {number}
     * @memberof PageOnlineUserInput
     */
    page?: number;
    /**
     * 页码容量
     * @type {number}
     * @memberof PageOnlineUserInput
     */
    pageSize?: number;
    /**
     * 排序字段
     * @type {string}
     * @memberof PageOnlineUserInput
     */
    field?: string | null;
    /**
     * 排序方向
     * @type {string}
     * @memberof PageOnlineUserInput
     */
    order?: string | null;
    /**
     * 降序排序
     * @type {string}
     * @memberof PageOnlineUserInput
     */
    descStr?: string | null;
    /**
     * 账号名称
     * @type {string}
     * @memberof PageOnlineUserInput
     */
    userName?: string | null;
    /**
     * 真实姓名
     * @type {string}
     * @memberof PageOnlineUserInput
     */
    realName?: string | null;
}
