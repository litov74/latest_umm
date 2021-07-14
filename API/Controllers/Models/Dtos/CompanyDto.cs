using API.Controllers.Models.Dtos;
using API.Database.Entities;
using System;
using static API.Common.Enums.CompanyAccessType;

namespace API.Models.Dtos
{
    public class CompanyDto: BaseDto
    {
        public string CompanyName { get; set; }
        public string LogoURL { get; set; }
        public bool IsEnabled { get; set; }
        public CompanyAccessTypes AccessType { get; set; }
        public virtual PlanSubscribeDto Subscription { get; set; }
        public virtual CompanyHierarchyItemsDto HierarchyItems { get; set; }
        public bool IsActive => IsEnabled && (AccessType == CompanyAccessTypes.Permanent ||
                               (Subscription?.ExpiryDate != null && Subscription.ExpiryDate.Value > DateTime.UtcNow));
    }
}
