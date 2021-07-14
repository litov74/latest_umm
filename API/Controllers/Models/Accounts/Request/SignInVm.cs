using System.ComponentModel.DataAnnotations;

namespace API.Controllers.Models.Accounts.Request
{
    public class SignInVm
    {
        [Required]
        [StringLength(254)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string Password { get; set; }
        public bool IsRememberMe { get; set; } = false;

    }
}
