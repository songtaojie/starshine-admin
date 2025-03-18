using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Starshine.Admin.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;

namespace Starshine.Admin.Account
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IdentityUserDto> RegisterAsync(RegisterInput input);

        Task SendPasswordResetCodeAsync(SendPasswordResetCodeInput input);

        Task<bool> VerifyPasswordResetTokenAsync(VerifyPasswordResetTokenInput input);

        Task ResetPasswordAsync(ResetPasswordInput input);
    }

}
