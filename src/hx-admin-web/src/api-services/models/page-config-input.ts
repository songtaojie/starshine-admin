/* tslint:disable */
/* eslint-disable */
/**
 * 所有接口
 * 让 .NET 开发更简单、更通用、更流行。前后端分离架构(.NET6/Vue3)，开箱即用紧随前沿技术。<br/><a href='https://gitee.com/zuohuaijun/Admin.NET/'>https://gitee.com/zuohuaijun/Admin.NET</a>
 *
 * OpenAPI spec version: 1.0.0
 * Contact: 515096995@qq.com
 *
 * NOTE: This class is auto generated by the swagger code generator program.
 * https://github.com/swagger-api/swagger-codegen.git
 * Do not edit the class manually.
 */
/**
 * 
 * @export
 * @interface PageConfigInput
 */
export interface PageConfigInput {
    /**
     * 当前页码
     * @type {number}
     * @memberof PageConfigInput
     */
    page?: number;
    /**
     * 页码容量
     * @type {number}
     * @memberof PageConfigInput
     */
    pageSize?: number;
    /**
     * 排序字段
     * @type {string}
     * @memberof PageConfigInput
     */
    field?: string | null;
    /**
     * 排序方向
     * @type {string}
     * @memberof PageConfigInput
     */
    order?: string | null;
    /**
     * 降序排序
     * @type {string}
     * @memberof PageConfigInput
     */
    descStr?: string | null;
    /**
     * 名称
     * @type {string}
     * @memberof PageConfigInput
     */
    name?: string | null;
    /**
     * 编码
     * @type {string}
     * @memberof PageConfigInput
     */
    code?: string | null;
    /**
     * 分组编码
     * @type {string}
     * @memberof PageConfigInput
     */
    groupCode?: string | null;
}
