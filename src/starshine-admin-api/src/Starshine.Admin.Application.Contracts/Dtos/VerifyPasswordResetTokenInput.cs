using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.Admin.Dtos
{
    public class VerifyPasswordResetTokenInput
    {
        public Guid UserId { get; set; }

        [Required]
        public required string ResetToken { get; set; }
    }

}
