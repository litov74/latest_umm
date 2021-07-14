using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.Models.Dtos.Stripe.Invoice
{
    public class PaymentPlan
    {
        public string Id { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }
        public string Interval { get; set; }
        public int IntervalCount { get; set; }
        public string Name { get; set; }
        public int? TrialPeriodDays { get; set; }
        public int CountUser { get; set; }
    }
}
