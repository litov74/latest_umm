using System.ComponentModel.DataAnnotations;

namespace API.Controllers.Models.Accounts.Request
{
    public class VerifyCodeVm
    {
        [Required]
        [StringLength(254)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Code { get; set; }
    }
}
