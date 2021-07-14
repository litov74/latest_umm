using API.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.Models.Dtos
{
    public class SubscriptionTierDto
    {
        public string Id { get; set; }
        public CompanySubscriptionPlanEnum TierName { get; set; }
        public decimal Price { get; set; }
        public int NumberOfEmployees { get; set; }
        public DateTime Monthly { get; set; }
        public DateTime Annually { get; set; }
        public bool CCAccess { get; set; }
        public bool CAAccess { get; set; }
    }
}
