using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.Models.Dtos.Stripe
{
    public class PaymentCoupon
    {
        public string Id { get; set; }
        public int? AmountOff { get; set; }
        public string Currency { get; set; }
        public string Duration { get; set; }
        public int? DurationInMonths { get; set; }
        public int? MaxRedemptions { get; set; }
        public int? PercentOff { get; set; }
        public int TimesRedeemed { get; set; }
        public bool Valid { get; set; }
    }
}
