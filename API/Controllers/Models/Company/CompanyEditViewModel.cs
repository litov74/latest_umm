using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static API.Common.Enums.CompanyAccessType;

namespace API.Controllers.Models.Company
{
    public class CompanyEditViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Name")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Admin Email")]
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        public Guid? BaseAdminId { get; set; }

        [Display(Name = "Access Type")]
        [Required]
        public CompanyAccessTypes AccessType { get; set; }

        [Display(Name = "Is Enabled")]
        [Required]
        public bool IsEnabled { get; set; }

        public IEnumerable<UserViewModel> Users { get; set; }

        [Display(Name = "Move user to")]
        public Guid? MoveToEntityId { get; set; }

        [Display(Name = "User")]
        public Guid? SourceUser { get; set; }

        [Display(Name = "Move initiatives to")]
        public Guid? TargetUser { get; set; }

        public int UserCount { get; set; }

        public int CompanySubcriptionUserCount { get; set; }
        public DateTime CompanySubcriptionExpriredDate { get; set; }
        public List<CompanyEditViewModel> MoveToEntityList { get; set; }
        public FreeSubscriptionModel FreeSubscriptionModel { get; set; }
        public CompanySubscriptionViewModel PaidSubscriptionModel { get; set; }
    }
}
