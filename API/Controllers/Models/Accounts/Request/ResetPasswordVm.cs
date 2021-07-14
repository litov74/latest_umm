using System.ComponentModel.DataAnnotations;

namespace API.Controllers.Models.Accounts.Request
{
    public class ResetPasswordVm
    {
        [Required]
        [StringLength(254)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Code { get; set; }
        [Required]

        [MinLength(6)]
        public string Password { get; set; }
    }
}
