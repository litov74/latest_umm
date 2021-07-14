using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.Models.Company
{
    public class CompanySubscriptionViewModel
    {
        public string Id { get; set; }
        public DateTime? CurrentPeriodStart { get; set; }
        public DateTime? CurrentPeriodEnd { get; set; }
        public int Amount { get; set; }
        public int CountUser { get; set; }
        public int IntervalCount { get; set; }
        public bool CancelAtPeriodEnd { get; set; }
        public int PricePerMonth
        {
            get
            {
                if (IntervalCount != 0)
                    return Amount / IntervalCount;
                else
                    return 0;
            }
        }
        public string SubscriptionType { get; set; }
        public string AccessType { get; set; }
    }
}
