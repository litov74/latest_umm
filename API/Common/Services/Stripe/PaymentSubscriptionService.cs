using API.Common.Interfaces;
using API.Common.Interfaces.Stripe;
using API.Controllers.Models.Dtos;
using API.Controllers.Models.Dtos.Stripe.Invoice;
using API.Data;
using API.Database.Entities;
using API.Shared;
using API.Utility;
using AutoMapper;
using Newtonsoft.Json;
using RestSharp;
using Stripe;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using static API.Shared.ApiFunctions;

namespace API.Common.Services.Stripe
{
    public class PaymentSubscriptionService:IPaymentSubscriptionService
    {
        private readonly string _stripeAPIKey;
        private readonly SubscriptionService _stripeSubscriptionService;
        private readonly CustomerService _stripeCustomerService;
        private readonly ChargeService _stripeChargeService;
        private readonly IPaymentPlanService _paymentPlanService;
        private readonly IPaymentInvoiceService _paymentInvoiceService;
        private readonly CardService _stripeCardService;
        private readonly InvoiceService _stripeInvoiceService;
        private const int QUANTITY = 1;
        private const int DAYSUNTILDUE = 15;
        private readonly string _stripeGSTAU;
        private readonly IMapper _mapper;
        private readonly string _stripeGSTNZ;
        private readonly APIContext _context;
        public PaymentSubscriptionService
       (
           IMapper mapper,
           ILogService logService,
           IPaymentPlanService paymentPlanService,
           IPaymentInvoiceService paymentInvoiceService,
           APIContext context
       )

        {
            _stripeAPIKey = ConfigurationManager.AppSettings["StripeApiKey"];
            _stripeSubscriptionService = new SubscriptionService();
            _stripeCustomerService = new CustomerService();
            _stripeChargeService = new ChargeService();
            _paymentPlanService = paymentPlanService;
            _paymentInvoiceService = paymentInvoiceService;
            _stripeInvoiceService = new InvoiceService();
            _stripeCardService = new CardService();
            _context = context;
            _mapper = mapper;
            _stripeGSTAU = ConfigurationManager.AppSettings["StripeGSTAU"];
            _stripeGSTNZ = ConfigurationManager.AppSettings["StripeGSTNZ"];
        }
        public async Task<Tuple<bool, PaymentInvoice>> AddOrUpdateSubscription(string companyId, string email, string token, string planId, string couponCode, bool useTrial = false)
        {
            var subscription = await _context.PlanSubscribeEntities.FindAsync(companyId);

            if (subscription == null)
            {
                subscription = new Database.Entities.PlanSubscribeEntity
                {
                    CompanyId = companyId.ToString()
                };
                _context.PlanSubscribeEntities.Add(subscription);
                if (useTrial)
                {
                    subscription.CountUser = StripeConfigurationParam.TrialUserCount;
                    subscription.ExpiryDate = DateTime.UtcNow.AddDays(StripeConfigurationParam.TrialInterval);
                    await _context.SaveChangesAsync();
                    return new Tuple<bool, PaymentInvoice>(true, new PaymentInvoice());
                }
            }
            else if (useTrial)
            {
                return new Tuple<bool, PaymentInvoice>(true, new PaymentInvoice());
            }

            Customer customer;
            if (string.IsNullOrWhiteSpace(subscription.UserId))
            {
                customer = await _stripeCustomerService.CreateAsync(new CustomerCreateOptions
                {
                    Email = email,
                    Source = token
                });
            }
            else
            {
                customer = await _stripeCustomerService.UpdateAsync(subscription.UserId, new CustomerUpdateOptions
                {
                    Email = email,
                    Source = token
                });
            }

            string taxId = "";
            var cardDefault = await _stripeCardService.GetAsync(customer.Id, customer.DefaultSourceId);
            if (cardDefault.Country == "AU")
            {
                taxId = _stripeGSTAU; // GST Autralia
            }
            else if (cardDefault.Country == "NZ")
            {
                taxId = _stripeGSTNZ; //GST New Zilane
            }

            subscription.UserId = customer.Id;
            PaymentInvoice invoiceRes = null;

            if (!string.IsNullOrWhiteSpace(subscription.SubscriptionTierId))
            {
                var isCancelSubscribe = await CancelSubscription(companyId, false);
                if (isCancelSubscribe)
                {
                    //for close old subscribe invoice
                    var invoices = await _paymentInvoiceService.GetInvoicesForCompany(companyId);
                    var invoiceOldSub = invoices.OrderByDescending(x => x.Date).FirstOrDefault(x => !x.Paid);

                    if (invoiceOldSub != null)
                        await _paymentInvoiceService.CloseInvoice(invoiceOldSub.Id);

                    var result = await UpdateSubscription(subscription, planId, couponCode, taxId);
                    invoiceRes = await PaymentAndGetInvoice(companyId);

                    return new Tuple<bool, PaymentInvoice>(result, invoiceRes);
                }
                return new Tuple<bool, PaymentInvoice>(false, null);
            }

            var stripeSubscription = await AddSubscription(subscription.UserId, planId, couponCode, useTrial, taxId);

            _mapper.Map(stripeSubscription, subscription);
            subscription.ExpiryDate = subscription.ExpiryDate.Value.AddDays(1);
            await _context.SaveChangesAsync();

            invoiceRes =await PaymentAndGetInvoice(companyId);

            return new Tuple<bool, PaymentInvoice>(stripeSubscription != null, invoiceRes);
        }
        public async Task<PaymentInvoice> PaymentAndGetInvoice(string companyId)
        {
            var invoices = await _paymentInvoiceService.GetInvoicesForCompany(companyId);
            var subscription = await _context.PlanSubscribeEntities.FindAsync(companyId);
            if (subscription == null)
            {
                throw new Exception("Company Id Doesn't Exist");
            }
            var invoiceFirstNotPaid = invoices.OrderByDescending(x => x.Date).FirstOrDefault(x => x.SubscriptionId == subscription.SubscriptionTierId && x.Paid);

            if (invoiceFirstNotPaid != null) 
            {
                return  await _paymentInvoiceService.GetInvoice(invoiceFirstNotPaid.Id);

            }
            else
                return new PaymentInvoice();

        }
        public async Task<CompanySubscriptionDto> GetSubscriptionById(string id)
        {
            try
            {
                var stripeSubscription = await _stripeSubscriptionService.GetAsync(id);
                return _mapper.Map<CompanySubscriptionDto>(stripeSubscription);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<ApiResponse<PlanSubscribeDto>> GetSubscriptionByCompanyId(string companyId)
        {
            var subscription = await _context.PlanSubscribeEntities.FindAsync(companyId);
            if (subscription == null)
            {
                throw new Exception("Company Id Doesn't Exist");
            }
            var subscriptionDto = _mapper.Map<PlanSubscribeDto>(subscription);
            return ApiSuccessResponse(subscriptionDto);
        }
        public async Task<bool> CancelSubscription(string companyId,  bool cancelAtPeriodEnd)
        {
            var subscription = await _context.PlanSubscribeEntities.FindAsync(companyId);
            if (subscription == null || string.IsNullOrWhiteSpace(subscription.SubscriptionTierId))
                return false;

            var isStripeSubscription = await _stripeSubscriptionService.GetAsync(subscription.SubscriptionTierId);
            if (isStripeSubscription.Status == "canceled")
                return true;

            var stripeSubscription = await _stripeSubscriptionService.CancelAsync(subscription.SubscriptionTierId,cancellationToken:default);
            return stripeSubscription != null;
        }
        public bool ValidateCompanyUserCount(int userCount, int actualUserCount)
        {
            var isValid = actualUserCount <= userCount;
            return isValid;
        }

        public bool CanAddNewUser(int userCount, int actualUserCount)
        {
            var isValid = actualUserCount < userCount;
            return isValid;
        }
        private async Task<bool> UpdateSubscription(PlanSubscribeEntity subscription, string newPlanId, string couponCode, string taxId = "")
        {
            var stripeSubscription = await _stripeSubscriptionService.GetAsync(subscription.SubscriptionTierId);

            if (stripeSubscription.Status == "active" || stripeSubscription.Status == "trialing")
            {
                var item = stripeSubscription.Items.Data.FirstOrDefault();

                var options = new SubscriptionUpdateOptions
                {
                    Items = new List<SubscriptionItemOptions>
                    {
                        new SubscriptionItemOptions { Id = item?.Id ?? "", Plan = newPlanId, Quantity = QUANTITY }
                    },
                    AutomaticTax = new SubscriptionAutomaticTaxOptions
                    {
                        Enabled = true,
                    },
                    Coupon = couponCode
                };
                stripeSubscription = await _stripeSubscriptionService.UpdateAsync(subscription.SubscriptionTierId, options);
            }
            else
            {
                //if subscription was deleted
                var newSubscription = await AddSubscription(subscription.UserId, newPlanId, couponCode, false, taxId);
                stripeSubscription = await _stripeSubscriptionService.GetAsync(newSubscription.Id);
            }
            _mapper.Map(stripeSubscription, subscription);
            subscription.ExpiryDate = subscription.ExpiryDate.Value.AddDays(1);
            await _context.SaveChangesAsync();

            return stripeSubscription != null;
        }
        private async Task<Subscription> AddSubscription(string customerId, string planId, string couponCode, bool useTrial = false, string taxId = "")
        {
            var stripeSubscription = await CreateStripeSubscriptionHttp(customerId, planId, couponCode, useTrial, taxId: taxId);

            return stripeSubscription;
        }
        private async Task<Subscription> CreateStripeSubscriptionHttp(string customerId, string planId, string couponCode, bool useTrial = false, string taxId = "")
        {
            var client = new RestClient("https://api.stripe.com/v1/subscriptions");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", $"Bearer {_stripeAPIKey}");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("customer", customerId);
            request.AddParameter("items[0][plan]", planId);
            request.AddParameter("collection_method", "charge_automatically");

            if (taxId.Length > 0)
            {
                request.AddParameter("default_tax_rates[0]", taxId);
            }

            if (couponCode.Length > 0)
            {
                request.AddParameter("coupon", couponCode);
            }
            if (useTrial)
            {
                var plan = await _paymentPlanService.GetPlanById(planId);
                if (plan != null)
                {
                    request.AddParameter("trial_period_days", plan.TrialPeriodDays);
                }
            }
            else
            {
                request.AddParameter("trial_end", "now");
            }

            var response = await client.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var res = JsonConvert.DeserializeObject<Subscription>(response.Content);
                return res;
            }
            else
            {
                throw new Exception("Create subcription error");
            }
        }
        

    }
}
