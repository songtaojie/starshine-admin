/* tslint:disable */
/* eslint-disable */

/**
 * 
 * @export
 * @interface AddSubscribeMessageTemplateInput
 */
export interface AddSubscribeMessageTemplateInput {
    /**
     * 模板标题Id
     * @type {string}
     * @memberof AddSubscribeMessageTemplateInput
     */
    templateTitleId: string;
    /**
     * 模板关键词列表,例如 [3,5,4]
     * @type {Array<number>}
     * @memberof AddSubscribeMessageTemplateInput
     */
    keyworkIdList: Array<number>;
    /**
     * 服务场景描述，15个字以内
     * @type {string}
     * @memberof AddSubscribeMessageTemplateInput
     */
    sceneDescription: string;
}
