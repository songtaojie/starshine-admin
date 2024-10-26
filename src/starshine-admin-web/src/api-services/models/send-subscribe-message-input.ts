/* tslint:disable */
/* eslint-disable */

import { DataItem } from './data-item';
/**
 * 
 * @export
 * @interface SendSubscribeMessageInput
 */
export interface SendSubscribeMessageInput {
    /**
     * 订阅模板Id
     * @type {string}
     * @memberof SendSubscribeMessageInput
     */
    templateId: string;
    /**
     * 接收者的OpenId
     * @type {string}
     * @memberof SendSubscribeMessageInput
     */
    toUserOpenId: string;
    /**
     * 模板内容，格式形如 { \"key1\": { \"value\": any }, \"key2\": { \"value\": any } }的object
     * @type {{ [key: string]: DataItem; }}
     * @memberof SendSubscribeMessageInput
     */
    data: { [key: string]: DataItem; };
    /**
     * 跳转小程序类型
     * @type {string}
     * @memberof SendSubscribeMessageInput
     */
    miniprogramState?: string | null;
    /**
     * 语言类型
     * @type {string}
     * @memberof SendSubscribeMessageInput
     */
    language?: string | null;
}
