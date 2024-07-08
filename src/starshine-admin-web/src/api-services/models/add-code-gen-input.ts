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
 * @interface AddCodeGenInput
 */
export interface AddCodeGenInput {
    /**
     * 当前页码
     * @type {number}
     * @memberof AddCodeGenInput
     */
    page?: number;
    /**
     * 页码容量
     * @type {number}
     * @memberof AddCodeGenInput
     */
    pageSize?: number;
    /**
     * 排序字段
     * @type {string}
     * @memberof AddCodeGenInput
     */
    field?: string | null;
    /**
     * 排序方向
     * @type {string}
     * @memberof AddCodeGenInput
     */
    order?: string | null;
    /**
     * 降序排序
     * @type {string}
     * @memberof AddCodeGenInput
     */
    descStr?: string | null;
    /**
     * 类名
     * @type {string}
     * @memberof AddCodeGenInput
     */
    className?: string | null;
    /**
     * 是否移除表前缀
     * @type {string}
     * @memberof AddCodeGenInput
     */
    tablePrefix?: string | null;
    /**
     * 库定位器名
     * @type {string}
     * @memberof AddCodeGenInput
     */
    configId?: string | null;
    /**
     * 数据库名(保留字段)
     * @type {string}
     * @memberof AddCodeGenInput
     */
    dbName?: string | null;
    /**
     * 数据库类型
     * @type {string}
     * @memberof AddCodeGenInput
     */
    dbType?: string | null;
    /**
     * 数据库链接
     * @type {string}
     * @memberof AddCodeGenInput
     */
    connectionString?: string | null;
    /**
     * 功能名（数据库表名称）
     * @type {string}
     * @memberof AddCodeGenInput
     */
    tableComment?: string | null;
    /**
     * 菜单应用分类（应用编码）
     * @type {string}
     * @memberof AddCodeGenInput
     */
    menuApplication?: string | null;
    /**
     * 数据库表名
     * @type {string}
     * @memberof AddCodeGenInput
     */
    tableName: string;
    /**
     * 业务名（业务代码包名称）
     * @type {string}
     * @memberof AddCodeGenInput
     */
    busName: string;
    /**
     * 命名空间
     * @type {string}
     * @memberof AddCodeGenInput
     */
    nameSpace: string;
    /**
     * 作者姓名
     * @type {string}
     * @memberof AddCodeGenInput
     */
    authorName: string;
    /**
     * 生成方式
     * @type {string}
     * @memberof AddCodeGenInput
     */
    generateType: string;
    /**
     * 菜单父级
     * @type {number}
     * @memberof AddCodeGenInput
     */
    menuPid: number;
}