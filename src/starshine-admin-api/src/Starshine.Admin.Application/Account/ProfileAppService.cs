using AutoMapper.Internal.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Starshine.Admin.Application.Contracts.Account;
using Starshine.Admin.Application.Contracts.Dtos.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Identity.Settings;
using Volo.Abp.Identity;
using Volo.Abp.Settings;
using Volo.Abp.Users;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending;

namespace Starshine.Admin.Account
{
    [Authorize]
    public class ProfileAppService : IdentityAppServiceBase, IProfileAppService
    {
        protected IdentityUserManager UserManager { get; }
        protected IOptions<IdentityOptions> IdentityOptions { get; }

        public ProfileAppService(
            IdentityUserManager userManager,
            IOptions<IdentityOptions> identityOptions)
        {
            UserManager = userManager;
            IdentityOptions = identityOptions;
        }

        public virtual async Task<ProfileOutput> GetAsync()
        {
            var currentUser = await UserManager.GetByIdAsync(CurrentUser.GetId());

            return ObjectMapper.Map<IdentityUser, ProfileOutput>(currentUser);
        }

        public virtual async Task<ProfileOutput> UpdateAsync(UpdateProfileInput input)
        {
            await IdentityOptions.SetAsync();

            var user = await UserManager.GetByIdAsync(CurrentUser.GetId());

            user.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);

            if (!string.Equals(user.UserName, input.UserName, StringComparison.InvariantCultureIgnoreCase))
            {
                if (await SettingProvider.IsTrueAsync(IdentitySettingNames.User.IsUserNameUpdateEnabled))
                {
                    (await UserManager.SetUserNameAsync(user, input.UserName)).CheckErrors();
                }
            }

            if (!string.Equals(user.Email, input.Email, StringComparison.InvariantCultureIgnoreCase))
            {
                if (await SettingProvider.IsTrueAsync(IdentitySettingNames.User.IsEmailUpdateEnabled))
                {
                    (await UserManager.SetEmailAsync(user, input.Email)).CheckErrors();
                }
            }

            if (user.PhoneNumber.IsNullOrWhiteSpace() && input.PhoneNumber.IsNullOrWhiteSpace())
            {
                input.PhoneNumber = user.PhoneNumber;
            }

            if (!string.Equals(user.PhoneNumber, input.PhoneNumber, StringComparison.InvariantCultureIgnoreCase))
            {
                (await UserManager.SetPhoneNumberAsync(user, input.PhoneNumber)).CheckErrors();
            }

            user.Name = input.Name;
            user.Surname = input.Surname;

            input.MapExtraPropertiesTo(user);

            (await UserManager.UpdateAsync(user)).CheckErrors();

            await CurrentUnitOfWork!.SaveChangesAsync();

            return ObjectMapper.Map<IdentityUser, ProfileOutput>(user);
        }

        public virtual async Task ChangePasswordAsync(ChangePasswordInput input)
        {
            await IdentityOptions.SetAsync();

            var currentUser = await UserManager.GetByIdAsync(CurrentUser.GetId());

            if (currentUser.IsExternal)
            {
                throw new BusinessException(code: IdentityErrorCodes.ExternalUserPasswordChange);
            }

            if (currentUser.PasswordHash == null)
            {
                (await UserManager.AddPasswordAsync(currentUser, input.NewPassword)).CheckErrors();

                return;
            }

            (await UserManager.ChangePasswordAsync(currentUser, input.CurrentPassword, input.NewPassword)).CheckErrors();
        }
    }
}
