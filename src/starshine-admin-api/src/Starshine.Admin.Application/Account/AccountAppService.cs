﻿using AutoMapper.Internal.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Starshine.Admin.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Settings;
using Volo.Abp;

namespace Starshine.Admin.Account
{
    public class AccountAppService : ApplicationService, IAccountAppService
    {
        protected IIdentityRoleRepository RoleRepository { get; }
        protected IdentityUserManager UserManager { get; }
        protected IAccountEmailer AccountEmailer { get; }
        protected IdentitySecurityLogManager IdentitySecurityLogManager { get; }
        protected IOptions<IdentityOptions> IdentityOptions { get; }

        public AccountAppService(
            IdentityUserManager userManager,
            IIdentityRoleRepository roleRepository,
            IAccountEmailer accountEmailer,
            IdentitySecurityLogManager identitySecurityLogManager,
            IOptions<IdentityOptions> identityOptions)
        {
            RoleRepository = roleRepository;
            AccountEmailer = accountEmailer;
            IdentitySecurityLogManager = identitySecurityLogManager;
            UserManager = userManager;
            IdentityOptions = identityOptions;

            LocalizationResource = typeof(AccountResource);
        }

        public virtual async Task<IdentityUserDto> RegisterAsync(RegisterInput input)
        {
            await CheckSelfRegistrationAsync();

            await IdentityOptions.SetAsync();

            var user = new IdentityUser(GuidGenerator.Create(), input.UserName, input.EmailAddress, CurrentTenant.Id);

            input.MapExtraPropertiesTo(user);

            (await UserManager.CreateAsync(user, input.Password)).CheckErrors();

            await UserManager.SetEmailAsync(user, input.EmailAddress);
            await UserManager.AddDefaultRolesAsync(user);

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }

        public virtual async Task SendPasswordResetCodeAsync(SendPasswordResetCodeInput input)
        {
            var user = await GetUserByEmailAsync(input.Email);
            var resetToken = await UserManager.GeneratePasswordResetTokenAsync(user);
            await AccountEmailer.SendPasswordResetLinkAsync(user, resetToken, input.AppName, input.ReturnUrl, input.ReturnUrlHash);
        }

        public virtual async Task<bool> VerifyPasswordResetTokenAsync(VerifyPasswordResetTokenInput input)
        {
            var user = await UserManager.GetByIdAsync(input.UserId);
            return await UserManager.VerifyUserTokenAsync(
                user,
                UserManager.Options.Tokens.PasswordResetTokenProvider,
                UserManager<IdentityUser>.ResetPasswordTokenPurpose,
                input.ResetToken);
        }

        public virtual async Task ResetPasswordAsync(ResetPasswordInput input)
        {
            await IdentityOptions.SetAsync();

            var user = await UserManager.GetByIdAsync(input.UserId);
            (await UserManager.ResetPasswordAsync(user, input.ResetToken, input.Password)).CheckErrors();

            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
            {
                Identity = IdentitySecurityLogIdentityConsts.Identity,
                Action = IdentitySecurityLogActionConsts.ChangePassword
            });
        }

        protected virtual async Task<IdentityUser> GetUserByEmailAsync(string email)
        {
            var user = await UserManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new UserFriendlyException(L["Volo.Account:InvalidEmailAddress", email]);
            }

            return user;
        }

        protected virtual async Task CheckSelfRegistrationAsync()
        {
            if (!await SettingProvider.IsTrueAsync(AccountSettingNames.IsSelfRegistrationEnabled))
            {
                throw new UserFriendlyException(L["SelfRegistrationDisabledMessage"]);
            }
        }
    }
}
