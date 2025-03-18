using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;

namespace Starshine.Admin.Dtos;

public class ResetPasswordInput
{
    public Guid UserId { get; set; }

    [Required]
    public string ResetToken { get; set; }

    [Required]
    [DisableAuditing]
    public string Password { get; set; }
}
