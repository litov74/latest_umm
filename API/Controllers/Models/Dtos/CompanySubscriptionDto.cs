using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.Models.Dtos
{
    public class CompanySubscriptionDto
    {
        public string Id { get; set; }
        public DateTime? CurrentPeriodStart { get; set; }
        public DateTime? CurrentPeriodEnd { get; set; }
        public int Amount { get; set; }
        public int CountUser { get; set; }
        public int IntervalCount { get; set; }
        public bool CancelAtPeriodEnd { get; set; }
    }
}
