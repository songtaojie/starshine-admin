using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.Admin.Models.Account
{
    public class LoginResult(LoginResultType type)
    {
        public LoginResultType Type { get; } = type;

        public string Description => Type.ToString();
    }

    public enum LoginResultType : byte
    {
        /// <summary>
        /// 登录成功
        /// </summary>
        Success = 1,

        /// <summary>
        /// 用户名或密码错误
        /// </summary>
        InvalidUserNameOrPassword = 2,

        /// <summary>
        /// 登录被禁止
        /// </summary>
        NotAllowed = 3,

        /// <summary>
        /// 锁定
        /// </summary>
        LockedOut = 4,

        /// <summary>
        /// 需要双因素认证
        /// </summary>
        RequiresTwoFactor = 5
    }
}
