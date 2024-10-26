/* tslint:disable */
/* eslint-disable */

/**
 * 
 * @export
 * @interface PageTenantInput
 */
export interface PageTenantInput {
    /**
     * 当前页码
     * @type {number}
     * @memberof PageTenantInput
     */
    page?: number;
    /**
     * 页码容量
     * @type {number}
     * @memberof PageTenantInput
     */
    pageSize?: number;
    /**
     * 排序字段
     * @type {string}
     * @memberof PageTenantInput
     */
    field?: string | null;
    /**
     * 排序方向
     * @type {string}
     * @memberof PageTenantInput
     */
    order?: string | null;
    /**
     * 降序排序
     * @type {string}
     * @memberof PageTenantInput
     */
    descStr?: string | null;
    /**
     * 名称
     * @type {string}
     * @memberof PageTenantInput
     */
    name?: string | null;
    /**
     * 电话
     * @type {string}
     * @memberof PageTenantInput
     */
    phone?: string | null;
}
