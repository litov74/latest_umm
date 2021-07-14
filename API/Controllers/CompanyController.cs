using API.Common.Interfaces;
using API.Common.Interfaces.Stripe;
using API.Controllers.Models.Company;
using API.Data;
using API.Shared;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static API.Shared.ApiFunctions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICompanyService _companyService;
        private readonly IPaymentSubscriptionService _paymentSubscriptionService;
        private readonly APIContext _context;
        public CompanyController(IMapper mapper, ICompanyService companyService, IPaymentSubscriptionService paymentSubscriptionService, ILogService logger,APIContext context) 
        {
            _mapper = mapper;
            _companyService = companyService;
            _paymentSubscriptionService = paymentSubscriptionService;
            _context = context;
        }
        [HttpGet("GetCompanyListById")]
        public async Task<ApiResponse<CompanyEditViewModel>> GetCompanyListById(string id)
        {
            try
            {
                var value = _companyService.GetCompanyById(id);
                var model = _mapper.Map<CompanyEditViewModel>(value);
                if (model == null)
                {
                    return ApiValidationResponse<CompanyEditViewModel>("Data not pulled Successfully");
                }

                FreeSubscriptionModel subscriptionModel = new FreeSubscriptionModel();
                //SetModelLists(model);
                //model.UserCount = _companyService.GetCompanyUserCount(id);
                var subscription = await _context.PlanSubscribeEntities.FindAsync(id);
                if (subscription != null)
                {
                    if (subscription.SubscriptionTierId == null)
                    {
                        subscriptionModel.Id = subscription.Id;
                        subscriptionModel.CustomerId = subscription.UserId;
                        subscriptionModel.SubscriptionId = subscription.SubscriptionTierId;
                        //subscriptionModel.PlanId = subscription.PlanId;
                        subscriptionModel.CountUser = subscription.CountUser;
                        subscriptionModel.ExpiryDate = subscription.ExpiryDate;
                        subscriptionModel.CreatedAt = subscription.CreatedAt;
                        subscriptionModel.LastModifiedAt = subscription.UpdatedAt;
                        //var freeSubscriptionModel =_mapper.Map<CompanySubscription>(subscription);
                        model.FreeSubscriptionModel = subscriptionModel;
                    }
                    else
                    {
                        var paidSubscription = await _paymentSubscriptionService.GetSubscriptionById(subscription.SubscriptionTierId);

                        var paidSubscriptionModel = _mapper.Map<CompanySubscriptionViewModel>(paidSubscription);
                        if (paidSubscriptionModel == null)
                        {
                            paidSubscriptionModel = new CompanySubscriptionViewModel();
                            paidSubscriptionModel.CurrentPeriodStart = subscription.CreatedAt;
                        }
                        paidSubscriptionModel.CountUser = subscription.CountUser;
                        paidSubscriptionModel.CurrentPeriodEnd = subscription.ExpiryDate;
                        model.PaidSubscriptionModel = paidSubscriptionModel;
                    }
                }
                else
                {
                    model.FreeSubscriptionModel = null;
                    model.PaidSubscriptionModel = null;
                }
                return ApiSuccessResponse(model);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        //[HttpPost("UpdateCompany")]
        //public HttpResponseMessage UpdateCompany(CompanyEditViewModel model)
        //{
         
        //    try
        //    {

        //        var value = _companyService.GetCompanyById(model.Id);

        //        if (value != null)
        //        {
        //            if (value.Subscription == null)
        //            {
        //                value.Subscription = new CompanySubscription
        //                {
        //                    Id = value.Id,
        //                    CountUser = 10,
        //                    ExpiryDate = DateTime.UtcNow.AddDays(14),
        //                    CreatedAt = DateTime.UtcNow,
        //                    LastModifiedAt = DateTime.UtcNow,
        //                    Company = value
        //                };
        //                _companyService.UpdateCompany(value);
        //            }
        //            _mapper.Map(model, value);
        //            if ((model.CompanySubcriptionUserCount != value.Subscription.CountUser || model.CompanySubcriptionExpriredDate != value.Subscription.ExpiryDate) && model.AccessType == CompanyAccessType.Subscription)
        //            {
        //                if (model.CompanySubcriptionUserCount != 0)
        //                {
        //                    value.Subscription.CountUser = model.CompanySubcriptionUserCount;

        //                }
        //                if (model.CompanySubcriptionExpriredDate != new DateTime())
        //                {
        //                    value.Subscription.ExpiryDate = model.CompanySubcriptionExpriredDate;
        //                }
        //                _companyService.UpdateCompany(value);
        //            }
        //            else
        //            {
        //                _companyService.UpdateCompany(value);
        //            }
        //            if (model.MoveToEntityId.HasValue && model.Id != model.MoveToEntityId)
        //            {
        //                var sourceUserCount = _companyService.GetCompanyUserCount(model.Id);
        //                if (sourceUserCount > 1 || sourceUserCount == 1)
        //                {
        //                    var user = _userService.GetById(model.SourceUser.Value);
        //                    user.IsBaseUserOfCompany = false;
        //                    _userManager.Update(user);
        //                    _userService.MoveUserToAnotherCompany(model.SourceUser.Value, model.TargetUser, model.MoveToEntityId.Value);

        //                    if (_userManager.IsInRole(user.Id, UserRoles.CompanyAdmin))
        //                    {
        //                        _userManager.RemoveFromRole(user.Id, UserRoles.CompanyAdmin);
        //                        _userManager.AddToRole(user.Id, UserRoles.CompanyUser);
        //                    }
        //                }
        //            }
        //            if ((model.BaseAdminId.HasValue && model.MoveToEntityId.HasValue && model.Id == model.MoveToEntityId) || (model.BaseAdminId.HasValue && model.SourceUser.HasValue == false) ||
        //                (model.BaseAdminId.HasValue && model.SourceUser.HasValue && model.BaseAdminId != model.SourceUser))
        //            {
        //                var oldBaseAdmin = _companyService.GetCompanyOwner(value.Id);
        //                if (oldBaseAdmin == null)
        //                {

        //                    var newBaseAdmin = _userService.GetById(model.BaseAdminId.Value);
        //                    _userService.RemoveRole(newBaseAdmin, UserRoles.CompanyUser);
        //                    _userService.AddRole(newBaseAdmin, UserRoles.CompanyAdmin);
        //                    newBaseAdmin.IsBaseUserOfCompany = true;
        //                    _userService.Update(newBaseAdmin);

        //                }
        //                else if (model.BaseAdminId.Value != oldBaseAdmin.Id)
        //                {
        //                    var newBaseAdmin = _userService.GetById(model.BaseAdminId.Value);
        //                    _userService.RemoveRole(newBaseAdmin, UserRoles.CompanyUser);
        //                    _userService.AddRole(newBaseAdmin, UserRoles.CompanyAdmin);
        //                    _companyService.MoveCompanyOwner(oldBaseAdmin, newBaseAdmin);
        //                }

        //            }

        //        }

        //        MemoryCacheHelper<object>.Delete(CacheKey.GetCompanyList);
        //        rm.message = ConstantsMessages.EditSuccess; //"Success: Company edited successfully.";                rm.returnUrl = "Index";
        //        rm.responseData = model;
        //        response = request.CreateResponse(HttpStatusCode.OK, new { rm, success = true });
        //    }
        //    catch (Exception ex)
        //    {
        //        rm.status = 0;
        //        rm.message = ex.Message;
        //        rm.responseData = model;
        //        response = request.CreateResponse(HttpStatusCode.OK, new { rm, success = false });
        //        _GenericLogStatement(APIConstantsPreFix.Company, APIConstantsRoute.UpdateCompany, ex);
        //    }
        //    return response;
        //}

    }
}
