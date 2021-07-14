using API.Common.Interfaces;
using API.Common.Interfaces.Stripe;
using API.Controllers.Models.Dtos.Stripe.Invoice;
using API.Utility;
using AutoMapper;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Common.Services.Stripe
{
    public class PaymentPlanService:IPaymentPlanService
    {
        private readonly PlanService _stripePlanService;
        private readonly IMapper _mapper;
        private readonly DateTime _updatePlansDate = new DateTime(2018, 5, 22);
        string value = "false";
        string userCounts = "1";

        public PaymentPlanService(IMapper mapper, ILogService logService)
        {
            _stripePlanService = new PlanService();
            _mapper = mapper;
        }
        public async Task<List<PaymentPlan>> GetPlans(int numberOfMonth, int? numberOfUsers = null)
        {
           
            var options = new ListOptions
            {
                Limit = 100
            };
            var plans = (await _stripePlanService.ListAsync((PlanListOptions)options))
                .Where(p => p.Interval == "month" && p.IntervalCount == numberOfMonth)
                .Where(p => p.Metadata.TryGetValue(StripeConfigurationParam.NewPlanMarker, out value))
                .Where(p => !numberOfUsers.HasValue || p.Metadata.TryGetValue(StripeConfigurationParam.UserCountParamName, out userCounts));

            return _mapper.Map<List<PaymentPlan>>(plans).OrderBy(p => p.CountUser).ToList();
        }
        public async Task<List<PaymentPlan>> GetPlanIntervals(int userCount)
        {
            var options = new ListOptions
            {
                Limit = 100
            };
            //var plans = await _stripePlanService.ListAsync(options);
            var plans = (await _stripePlanService.ListAsync((PlanListOptions)options))
                .Where(p => p.Metadata.TryGetValue(StripeConfigurationParam.NewPlanMarker, out value))
                .Where(p => p.Metadata.TryGetValue(StripeConfigurationParam.UserCountParamName, out userCounts))
                .OrderBy(p => p.IntervalCount);

            return _mapper.Map<List<PaymentPlan>>(plans);
        }
        public async Task<List<PaymentPlan>> GetPlanViewerIntevals()
        {
            var options = new ListOptions
            {
                Limit = 100,
            };

            var plans = (await _stripePlanService.ListAsync((PlanListOptions)options))
                .Where(p => p.Metadata.TryGetValue("isViewerPlan", out value))
                .OrderBy(p => p.IntervalCount);
            return _mapper.Map<List<PaymentPlan>>(plans);
        }
        public async Task<PaymentPlan> GetPlanById(string id)
        {
            var plan = await _stripePlanService.GetAsync(id);
            return _mapper.Map<PaymentPlan>(plan);
        }
    }
}
