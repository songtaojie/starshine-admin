using Starshine.Admin.Application.Contracts.Dtos.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Starshine.Admin.Application.Contracts.Account
{
    public interface IProfileAppService : IApplicationService
    {
        Task<ProfileOutput> GetAsync();

        Task<ProfileOutput> UpdateAsync(UpdateProfileInput input);

        Task ChangePasswordAsync(ChangePasswordInput input);
    }
}
