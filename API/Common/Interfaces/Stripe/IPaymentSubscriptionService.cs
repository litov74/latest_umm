using API.Controllers.Models.Dtos;
using API.Controllers.Models.Dtos.Stripe.Invoice;
using API.Database.Entities;
using API.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Common.Interfaces.Stripe
{
    public interface IPaymentSubscriptionService
    {
        Task<CompanySubscriptionDto> GetSubscriptionById(string id);
        Task<ApiResponse<PlanSubscribeDto>> GetSubscriptionByCompanyId(string companyId);
        Task<Tuple<bool, PaymentInvoice>> AddOrUpdateSubscription(string companyId, string email, string token, string planId, string couponCode, bool useTrial = false);
        Task<bool> CancelSubscription(string companyId, bool cancelAtPeriodEnd);
        bool ValidateCompanyUserCount(int userCount, int actualUserCount);
        bool CanAddNewUser(int userCount, int actualUserCount);

    }
}
