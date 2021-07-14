using System.ComponentModel.DataAnnotations;

namespace API.Controllers.Models.Accounts.Request
{
    public class ForgotAndResendVm
    {
        [Required]
        [StringLength(254)]
        [EmailAddress]
        public string Email { get; set; }
    }
}
