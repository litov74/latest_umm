using API.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.Models.Dtos
{
    public class PlanSubscribeDto { 
    
        public string Id { get; set; }
        public string Plan { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public int CountUser { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool IsPaymentVerified { get; set; }
        public UserDto User { get; set; }
        public SubscriptionTierDto SubscriptionTier { get; set; }
        public CompanyDto Company { get; set; }
    }
}
