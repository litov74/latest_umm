using API.Common.Enums;
using API.Common.Interfaces;
using API.Common.Interfaces.Stripe;
using API.Common.Models;
using API.Common.Services;
using API.Controllers.Models.Dtos;
using API.Controllers.Models.Dtos.Stripe;
using API.Controllers.Models.Dtos.Stripe.Invoice;
using API.Data;
using API.Models.Dtos;
using API.Shared;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static API.Common.Enums.CompanyAccessType;
using static API.Shared.ApiFunctions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentPlanService _paymentPlanService;
        private readonly IPaymentSubscriptionService _paymentSubscriptionService;
        private readonly IPaymentCouponService _paymentCouponService;
        private readonly SendGridEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IUserClaimService _userClaimService;
        private readonly ActiveCampaignService _activeCampaignService;
        private readonly APIContext _context;
        public PaymentController
        (
            IPaymentPlanService paymentPlan,
            IPaymentSubscriptionService paymentSubscriptionService,
            IPaymentInvoiceService paymentInvoiceService,
            IPaymentCouponService paymentCouponService,
            SendGridEmailService emailService,
            ILogService _logger,
            IMapper mapper,
            ActiveCampaignService activeCampaignService,
            IUserClaimService userClaimService,
            APIContext context
        )
        {
            _paymentPlanService = paymentPlan;
            _paymentSubscriptionService = paymentSubscriptionService;
            _paymentCouponService = paymentCouponService;
            _emailService = emailService;
            _mapper = mapper;
            _activeCampaignService = activeCampaignService;
            _userClaimService = userClaimService;
            _context = context;
        }
        [HttpPost("GetSelectPlan")]
        public async Task<ActionResult<ApiResponse<PlanSelectionViewModel>>> GetSelectPlan(string userEmail)
        {

            //HttpResponseMessage response = null;
            try
            {
                var currentUserId = _userClaimService.GetCurrentUserId();
                var findEmailId = _context.UserEntity.FirstOrDefaultAsync(x => x.Email == userEmail);
                if (findEmailId == null)
                {
                    return BadRequest("Email Not Found!");
                }
                var listsModel = await _paymentPlanService.GetPlans(1);
                var canUseTrial = await _context.PlanSubscribeEntities.Where(x => x.UserId == currentUserId).Select(x => new
                {
                    x.Plan,
                    x.SubscriptionTierId
                }).FirstOrDefaultAsync();
                var freeTrial = canUseTrial == null || (!string.IsNullOrWhiteSpace(canUseTrial.SubscriptionTierId) && await _paymentSubscriptionService.GetSubscriptionById(canUseTrial.SubscriptionTierId) == null);
                var model = new PlanSelectionViewModel()
                {
                    PaymentListsModel = listsModel,
                    CanUseTrial = freeTrial
                };

                return ApiSuccessResponse(model);
            }
            catch (Exception ex)
            {
                throw;
            }

            return null;
        }
        [HttpPost("SelectPlan")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseJson<UserDto>))]
        public async Task<ActionResult<ApiResponse<string>>> SelectPlan(PlanSelectionViewModel model)
        {
            var getCurrentUserEmail = _userClaimService.GetCurrentUserEmail();
            var getCurrentUserCompany = _userClaimService.GetCurrentUserCompanyId();
            try
            {
                if (model.UseFreeTrial || ModelState.IsValid)
                {
                    var user = await _context.UserEntity.FirstOrDefaultAsync(x => x.Email == getCurrentUserEmail);
                    if (user == null)
                    {
                        return ApiValidationResponse<string>("User Not Found!");
                    }
                    
                    if (user != null)
                    {
                        var planSubscribe = await _context.PlanSubscribeEntities.Where(x => x.UserId == user.Id).ToListAsync();
                        var company = _context.CompanyEntity.FirstOrDefault(x => x.Id == getCurrentUserCompany);
                        if (company.AccessType == CompanyAccessTypes.Permanent)
                        {
                            await _emailService.RemoveContactFromListTrial(user.Email);
                            var contactId = await _activeCampaignService.CreateOrUpdateContact(user.Email, user.FirstName, user.LastName);
                            await _activeCampaignService.UpdateContactListTrial(contactId, ContactListStatus.Unsubscribed);

                            return ApiSuccessResponse("Success");
                        }
                        var result = await _paymentSubscriptionService.AddOrUpdateSubscription(company.Id, model.stripeEmail, model.stripeToken, model.PlanId, model.stripeCouponCode, model.UseFreeTrial);

                        if (result.Item2 == null)
                        {
                            return ApiValidationResponse<string>("Payment Faild!");
                        }
                       
                        if (result.Item1)
                        {
                            PaymentInvoiceReceiptViewModel modelViewModel = null;

                            if (result.Item2 != null)
                            {
                                var companySubscraption = _mapper.Map<PlanSubscribeDto>(planSubscribe);

                                modelViewModel = new PaymentInvoiceReceiptViewModel
                                {
                                    PlanSubscribeDto = companySubscraption,
                                    PaymentInvoice = result.Item2
                                };
                            }
                            
                            if (model.UseFreeTrial)
                            {
                                await _emailService.AddOrUpdateEmailToSendGirdTrialList(user.Email, user.FirstName, user.LastName);
                                var contactId = await _activeCampaignService.CreateOrUpdateContact(user.Email, user.FirstName, user.LastName);
                                await _activeCampaignService.UpdateContactListTrial(contactId, ContactListStatus.Active);

                            }
                            else
                            {
                                await _emailService.RemoveContactFromListTrial(user.Email);
                                var contactId = await _activeCampaignService.CreateOrUpdateContact(user.Email, user.FirstName, user.LastName);
                                await _activeCampaignService.UpdateContactListTrial(contactId, ContactListStatus.Unsubscribed);
                            }

                            return ApiSuccessResponse("Payment Successfull!");
                        }
                    }
                }
                model.PaymentListsModel = await _paymentPlanService.GetPlans(1);
                return ApiValidationResponse<string>("Validation Faild!");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Authorize]
        [HttpPost("PlanInterval")]
        public async Task<ActionResult<ApiResponse<List<PaymentPlan>>>> PlanInterval(int userCount)
        {
            try
            {
                var listsModel = await _paymentPlanService.GetPlanIntervals(userCount);
                return ApiSuccessResponse(listsModel);
            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }
        [Authorize]
        [HttpGet]
        [Route("GetPlanViewerInterval")]
        public async Task<ActionResult<ApiResponse<List<PaymentPlan>>>> GetPlanViewerInterval()
        {
            
            try
            {
                var userCount = 10;
                var listsModel = await _paymentPlanService.GetPlanViewerIntevals();
                return ApiSuccessResponse(listsModel);
            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }
        [HttpGet]
        [Route("ValidateCoupon")]
        public async Task<ActionResult<ApiResponse<PaymentCoupon>>> ValidateCoupon(string coupon)
        {
            try
            {
                var paymentCoupon = _paymentCouponService.GetCouponId(coupon);
                if (!paymentCoupon.Valid)
                {
                    return ApiSuccessResponse(paymentCoupon);
                }
                return ApiSuccessResponse(paymentCoupon);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
       

    }
}
