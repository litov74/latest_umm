using API.Common.Enums;
using API.Common.Interfaces;
using API.Controllers.Models.Dtos;
using API.Controllers.Models.PlanSubscribe.Request;
using API.Data;
using API.Database.Entities;
using API.Models.Entities;
using API.Shared;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static API.Shared.ApiFunctions;

namespace API.Common.Services
{
    public class PlanSubscribeService : IPlanSubscribeService
    {
        private readonly APIContext _context;
        private readonly IUserClaimService _userClaimService;
        private readonly IMapper _mapper;
        Dictionary<string, string> additionalClaims = new Dictionary<string, string>();
        public PlanSubscribeService(APIContext context, IUserClaimService userClaimService, IMapper mapper)
        {
            _context = context;
            _userClaimService = userClaimService;
            _mapper = mapper;
        }
        public async Task<ApiResponse<string>> CreatePlanSubscribe(PlanSubscribeVm planSubscribeVm)
        {
            var subscriptionPlan = "";
            var currentUserId = _userClaimService.GetCurrentUserId();
            var user = await _context.UserEntity
              .Include(x => x.CompanyMappings)
              .ThenInclude(x => x.Company)
              .FirstOrDefaultAsync(x => x.Id == currentUserId && !x.IsDeleted && x.Email == planSubscribeVm.Email);
            var company = await _context.CompanyEntity.Where(x => x.CompanyName == planSubscribeVm.CompanyName.ToLower().Trim()).Select(x => new
            {
                x.Id
            }).FirstOrDefaultAsync();

            var subscribesTiername =  _context.SubscriptionTierEntities.Where(x => x.Id == planSubscribeVm.SubscriptionTierId).Select(x => x.TierName).FirstOrDefault();

            //subscriptionPlan = subscribesTiername.ToString();
            if (subscribesTiername == null)
            {
                return ApiValidationResponse<string>("Subscription TireId Not Found!");
            }
            if (company == null)
            {
                return ApiValidationResponse<string>("Company Name Missmatch!");
            }
           
            if (subscribesTiername == CompanySubscriptionPlanEnum.Trial)
            {
                return ApiSuccessResponse("Congratulations Your Free Trial Subscription Plan is Activated Successfully");
            }
            if (user != null)
            {
                var planSubscriber = new PlanSubscribeEntity();
               
                    planSubscriber = new PlanSubscribeEntity
                    {
                        UserId = currentUserId,
                        FullName = $"{user.FirstName} {user.LastName}",
                        Email = user.Email,
                        SubscriptionTierId = user.SubscriptionTierId,
                        Plan = subscribesTiername.ToString(),
                        CompanyId = company.Id.ToString(),
                        CountUser = planSubscribeVm.CountUser,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = user.Email
                    };
                    await _context.AddAsync(planSubscriber);
                    await _context.SaveChangesAsync();
                

                return ApiSuccessResponse($"Your are Successfully Subscribed to {planSubscriber.Plan} Plan.");
            }
            return ApiValidationResponse<string>("", "Email Address Or UserId Not Found");
        }

        public async Task<ApiResponse<List<PlanSubscribeDto>>> GetAllPlanSubscribeUserHistory()
        {
            var getCurrentUser = _userClaimService.GetCurrentUserId();
            var getCurrentUserRole = _userClaimService.GetCurrentUserRole();
            var currentUserId = await _context.PlanSubscribeEntities.Where(x => x.UserId == getCurrentUser).ToListAsync();
            if (currentUserId.Count == 0)
            {
                return ApiErrorResponse<List<PlanSubscribeDto>>("User Not Found!");
            }

            if (getCurrentUserRole == Roles.User)
            {
                var planDto = _mapper.Map<List<PlanSubscribeDto>>(currentUserId);
                return ApiSuccessResponse(planDto);
            }

            var getAllSubscribedList = await _context.PlanSubscribeEntities.ToListAsync();
            var DtoList = _mapper.Map<List<PlanSubscribeDto>>(getAllSubscribedList);
            return ApiSuccessResponse(DtoList);
        }

    }
}
