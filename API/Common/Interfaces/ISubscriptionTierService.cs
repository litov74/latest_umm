using API.Controllers.Models.Accounts.Request;
using API.Controllers.Models.Dtos;
using API.Controllers.Models.SubscriptionTier.Request;
using API.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Common.Interfaces
{
    public interface ISubscriptionTierService
    {
        Task<ApiResponse<string>> CreateSubscriptionTier(SubscriptionTierVm subscriptionTierVm);
        Task<ApiResponse<List<SubscriptionTierDto>>> GetAllSubscriptionTier();
        Task<ApiResponse<bool>> DeleteSubscriptionTier(string Id);
        Task<ApiResponse<SubscriptionTierDto>> GetSubscriptionTierById(string Id);
        Task<ApiResponse<string>> UpdateSubscriptionTier(SubscriptionTierVm subscriptionTierVm);
    }
}
