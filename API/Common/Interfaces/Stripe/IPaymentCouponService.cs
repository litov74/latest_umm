using API.Controllers.Models.Dtos.Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Common.Interfaces.Stripe
{
    public interface IPaymentCouponService
    {
        PaymentCoupon GetCouponId(string id);
    }
}
