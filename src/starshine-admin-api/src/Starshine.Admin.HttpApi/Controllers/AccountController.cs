using Microsoft.AspNetCore.Mvc;
using Starshine.Admin.Account;
using Starshine.Admin.Dtos;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;
using Volo.Abp;
using Starshine.Admin.Consts;

namespace Starshine.Admin.Controllers
{
    [RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
    [Area(AccountRemoteServiceConsts.ModuleName)]
    [Route("api/account")]
    public class AccountController(IAccountAppService accountAppService) : AbpControllerBase
    {
        [HttpPost]
        [Route("register")]
        public virtual Task<IdentityUserDto> RegisterAsync(RegisterInput input)
        {
            return accountAppService.RegisterAsync(input);
        }

        [HttpPost]
        [Route("send-password-reset-code")]
        public virtual Task SendPasswordResetCodeAsync(SendPasswordResetCodeInput input)
        {
            return accountAppService.SendPasswordResetCodeAsync(input);
        }

        [HttpPost]
        [Route("verify-password-reset-token")]
        public virtual Task<bool> VerifyPasswordResetTokenAsync(VerifyPasswordResetTokenInput input)
        {
            return accountAppService.VerifyPasswordResetTokenAsync(input);
        }

        [HttpPost]
        [Route("reset-password")]
        public virtual Task ResetPasswordAsync(ResetPasswordInput input)
        {
            return accountAppService.ResetPasswordAsync(input);
        }
    }
}
