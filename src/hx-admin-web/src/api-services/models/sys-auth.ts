/**
 * 用户登录参数
 * @export
 * @interface LoginInput
 */
 export interface LoginInput {
    /**
     * 账号
     * @type {string}
     * @memberof LoginInput
     */
    account: string;
    /**
     * 密码
     * @type {string}
     * @memberof LoginInput
     */
    password: string;
    /**
     * 验证码Id
     * @type {number}
     * @memberof LoginInput
     */
    codeId?: number;
    /**
     * 验证码
     * @type {string}
     * @memberof LoginInput
     */
    code?: string | null;
}

/**
 * 用户登录结果
 * @export
 * @interface LoginOutput
 */
 export interface LoginOutput {
    /**
     * 令牌Token
     * @type {string}
     * @memberof LoginOutput
     */
    accessToken?: string | null;
    /**
     * 刷新Token
     * @type {string}
     * @memberof LoginOutput
     */
    refreshToken?: string | null;
}

/**
 * 用户登录信息
 * @export
 * @interface LoginUserOutput
 */
 export interface LoginUserOutput {
    /**
     * 账号名称
     * @type {string}
     * @memberof LoginUserOutput
     */
    account?: string | null;
    /**
     * 真实姓名
     * @type {string}
     * @memberof LoginUserOutput
     */
    realName?: string | null;
    /**
     * 头像
     * @type {string}
     * @memberof LoginUserOutput
     */
    avatar?: string | null;
    /**
     * 个人简介
     * @type {string}
     * @memberof LoginUserOutput
     */
    introduction?: string | null;
    /**
     * 地址
     * @type {string}
     * @memberof LoginUserOutput
     */
    address?: string | null;
    /**
     * 电子签名
     * @type {string}
     * @memberof LoginUserOutput
     */
    signature?: string | null;
    /**
     * 机构Id
     * @type {number}
     * @memberof LoginUserOutput
     */
    orgId?: number;
    /**
     * 机构名称
     * @type {string}
     * @memberof LoginUserOutput
     */
    orgName?: string | null;
    /**
     * 职位名称
     * @type {string}
     * @memberof LoginUserOutput
     */
    posName?: string | null;
    /**
     * 按钮权限集合
     * @type {Array<string>}
     * @memberof LoginUserOutput
     */
    buttons?: Array<string> | null;
}
