using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ObjectExtending;

namespace Starshine.Admin.Application.Contracts.Dtos.Profiles
{
    public class ProfileOutput : ExtensibleObject, IHasConcurrencyStamp
    {
        public string? UserName { get; set; }

        public string? Email { get; set; }

        public string? Name { get; set; }

        public string? Surname { get; set; }

        public string? PhoneNumber { get; set; }

        public bool IsExternal { get; set; }

        public bool HasPassword { get; set; }

        public string? ConcurrencyStamp { get; set; }
    }
}
