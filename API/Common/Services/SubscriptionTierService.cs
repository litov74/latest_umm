using API.Common.Interfaces;
using API.Controllers.Models.Dtos;
using API.Controllers.Models.SubscriptionTier.Request;
using API.Data;
using API.Database.Entities;
using API.Shared;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static API.Shared.ApiFunctions;

namespace API.Common.Services
{
    public class SubscriptionTierService : ISubscriptionTierService
    {
        private readonly IUserClaimService _userClaimService;
        private readonly APIContext _context;
        private readonly IMapper _mapper;
        public SubscriptionTierService(IUserClaimService userClaimService, APIContext context, IMapper mapper)
        {
            _userClaimService = userClaimService;
            _context = context;
            _mapper = mapper;

        }
        public async Task<ApiResponse<string>> CreateSubscriptionTier(SubscriptionTierVm subscriptionTierVm)
        {
            var currentUserId = _userClaimService.GetCurrentUserId();

            var user = await Task.FromResult(_context.UserEntity.FirstOrDefault(x => x.Id == currentUserId));
            if (user == null)
            {
                return ApiErrorResponse<string>("User Id Not Found!");
            }
            var subscriptionTier = new SubscriptionTierEntity()
            {
                TierName = subscriptionTierVm.TierName,
                Price = subscriptionTierVm.Price,
                NumberOfEmployees = subscriptionTierVm.NumberOfEmployees,
                Monthly = subscriptionTierVm.Monthly,
                Annually = subscriptionTierVm.Annually,
                CCAccess = subscriptionTierVm.CCAccess,
                CAAccess = subscriptionTierVm.CAAccess,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = user.Email
            };
            await _context.AddAsync(subscriptionTier);
            await _context.SaveChangesAsync();
            return ApiSuccessResponse($"You Are Successfully Created {subscriptionTier.TierName} Plan");
        }
        public async Task<ApiResponse<List<SubscriptionTierDto>>> GetAllSubscriptionTier()
        {
            var getAllTier = await _context.SubscriptionTierEntities.ToListAsync();
            var tierDto = _mapper.Map<List<SubscriptionTierDto>>(getAllTier);
            return ApiSuccessResponse(tierDto);
        }
        public async Task<ApiResponse<bool>> DeleteSubscriptionTier(string Id)
        {
            var findId = await _context.SubscriptionTierEntities.FirstOrDefaultAsync(x => x.Id == Id);
            if(findId == null)
            {
                return ApiErrorResponse<bool>("Record Not Found!");
            }
             _context.SubscriptionTierEntities.Remove(findId);
            await _context.SaveChangesAsync();
            return ApiSuccessResponse(true);
        }
        public async Task<ApiResponse<SubscriptionTierDto>> GetSubscriptionTierById(string Id)
        {
            var getTier = await _context.SubscriptionTierEntities.FindAsync(Id);
            var tierDto = _mapper.Map<SubscriptionTierDto>(getTier);
            return ApiSuccessResponse(tierDto);
        }
        public async Task<ApiResponse<string>> UpdateSubscriptionTier(SubscriptionTierVm subscriptionTierVm)
        {
            var currentUserId = _userClaimService.GetCurrentUserId();
            var tier = await Task.FromResult(_context.SubscriptionTierEntities.FirstOrDefault(x => x.Id == subscriptionTierVm.Id));
            if (tier == null)
            {
                return ApiErrorResponse<string>("User Id Not Found!");
            }
            tier.TierName = subscriptionTierVm.TierName;
            tier.Price = subscriptionTierVm.Price;
            tier.NumberOfEmployees = subscriptionTierVm.NumberOfEmployees;
            tier.Monthly = subscriptionTierVm.Monthly;
            tier.Annually = subscriptionTierVm.Annually;
            tier.CCAccess = subscriptionTierVm.CCAccess;
            tier.CAAccess = subscriptionTierVm.CAAccess;
            tier.UpdatedAt = DateTime.UtcNow;
            tier.UpdatedBy = currentUserId;
            
            await _context.SaveChangesAsync();
            return ApiSuccessResponse($"You Are Successfully Updated {tier.TierName} Plan");
        }

    }
}
