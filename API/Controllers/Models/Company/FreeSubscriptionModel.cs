using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.Models.Company
{
    public class FreeSubscriptionModel
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string SubscriptionId { get; set; }
        //public string PlanId { get; set; }
        public int CountUser { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public string SubscriptionType { get; set; }
        public string AccessType { get; set; }
    }
}
