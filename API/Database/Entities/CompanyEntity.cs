using API.Database.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using static API.Common.Enums.CompanyAccessType;

namespace API.Models.Entities
{
    public class CompanyEntity : BaseStringIdEntity
    {
        [MaxLength(512)]
        public string CompanyName { get; set; }

        [MaxLength(2048)]
        public string LogoURL { get; set; }
        public bool IsEnabled { get; set; }
        public CompanyAccessTypes AccessType { get; set; }
        public virtual PlanSubscribeEntity Subscription { get; set; }
        public virtual CompanyHierarchyItem HierarchyItems { get; set; }
        public bool IsActive => IsEnabled && (AccessType == CompanyAccessTypes.Permanent ||
                               (Subscription?.ExpiryDate != null && Subscription.ExpiryDate.Value > DateTime.UtcNow));
    }
}
