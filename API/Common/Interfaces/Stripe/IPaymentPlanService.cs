using API.Controllers.Models.Dtos.Stripe.Invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Common.Interfaces.Stripe
{
    public interface IPaymentPlanService
    {
        Task<PaymentPlan> GetPlanById(string id);
        Task<List<PaymentPlan>> GetPlans(int numberOfMonth, int? numberOfUsers = null);
        Task<List<PaymentPlan>> GetPlanIntervals(int userCount);
        Task<List<PaymentPlan>> GetPlanViewerIntevals();
    }
}
