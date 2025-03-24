﻿using Microsoft.AspNetCore.Mvc;
using Starshine.Admin.Dtos;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;
using Volo.Abp;
using Starshine.Admin.Consts;
using Microsoft.AspNetCore.Identity;
using Starshine.Admin.Models.Account;
using Volo.Abp.Settings;
using Starshine.Admin.Application.Contracts.Account;
using Starshine.Admin.Application.Contracts.Account.Settings;
using System;
using Volo.Abp.Validation;
using Microsoft.Extensions.Options;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Starshine.Admin.Controllers
{
    [RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
    [Area(AccountRemoteServiceConsts.ModuleName)]
    [Route("api/account")]
    public class AccountController(IAccountAppService accountAppService, 
        ISettingProvider settingProvider,
        IIdentityUserAppService identityUserAppService,
        IOptions<IdentityOptions> identityOptions) : AbpControllerBase
    {
        #region 登录
        [HttpPost]
        [Route("login")]
        public virtual async Task<LoginResult> Login(UserLoginInput login)
        {
            await CheckLocalLoginAsync();

            ValidateLoginInfo(login);

            await ReplaceEmailToUsernameOfInputIfNeeds(login);

            await identityOptions.SetAsync();

            var signInResult = await SignInManager.PasswordSignInAsync(
                login.UserNameOrEmailAddress,
                login.Password,
                login.RememberMe,
                true
            );

            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
            {
                Identity = IdentitySecurityLogIdentityConsts.Identity,
                Action = signInResult.ToIdentitySecurityLogAction(),
                UserName = login.UserNameOrEmailAddress
            });

            if (signInResult.Succeeded)
            {
                var user = await UserManager.FindByNameAsync(login.UserNameOrEmailAddress);
                if (user != null)
                {
                    // Clear the dynamic claims cache.
                    await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(user.Id, user.TenantId);
                }
            }

            return GetAbpLoginResult(signInResult);
        }

       
        #endregion


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
