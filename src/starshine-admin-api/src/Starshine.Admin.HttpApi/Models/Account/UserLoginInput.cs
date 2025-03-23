using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Auditing;

namespace Starshine.Admin.Models.Account
{
    public class UserLoginInput
    {
        [Required]
        [StringLength(255)]
        public required string UserNameOrEmailAddress { get; set; }

        [Required]
        [StringLength(32)]
        [DataType(DataType.Password)]
        [DisableAuditing]
        public required string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
