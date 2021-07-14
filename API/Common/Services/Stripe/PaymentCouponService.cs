using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Common.Interfaces.Stripe;
using API.Controllers.Models.Dtos.Stripe;
using AutoMapper;
using Stripe;

namespace API.Common.Services.Stripe
{
    public class PaymentCouponService : IPaymentCouponService
    {
        private readonly CouponService _stripeCouponService;
        private readonly IMapper _mapper;
        public PaymentCouponService(IMapper mapper)
        {
            _stripeCouponService = new CouponService();
            _mapper = mapper;

        }
        public PaymentCoupon GetCouponId(string id)
        {
            var coupon = _stripeCouponService.Get(id);
            return new PaymentCoupon
            {
                Id = coupon.Id,
                AmountOff = Convert.ToInt32(coupon.AmountOff),
                Currency = coupon.Currency,
                Duration = coupon.Duration,
                DurationInMonths = Convert.ToInt32(coupon.DurationInMonths),
                MaxRedemptions = Convert.ToInt32(coupon.MaxRedemptions),
                PercentOff = Convert.ToInt32(coupon.PercentOff),
                TimesRedeemed = Convert.ToInt32(coupon.TimesRedeemed),
                Valid = coupon.Valid
            };
        }
    }
}
