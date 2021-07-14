using API.Common.Interfaces.Stripe;
using API.Data;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Common.Services.Stripe
{
    public class PaymentCustomerService:IPaymentCustomerService
    {
        private readonly CustomerService _stripeCustomerService;
        private readonly APIContext _context;
        public PaymentCustomerService(APIContext context)
        {
            _stripeCustomerService = new CustomerService();
            _context = context;
        }
        public async Task<bool> AddOrUpdateCustomer(string companyId, string email, string token)
        {
            try
            {
                var subscription = await _context.PlanSubscribeEntities.FindAsync(companyId);

                if (subscription == null)
                {
                    subscription = new Database.Entities.PlanSubscribeEntity
                    {
                        CompanyId = companyId
                    };
                    _context.PlanSubscribeEntities.Add(subscription);
                }

                Customer customer;
                if (subscription.UserId == null)
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
                subscription.UserId = customer.Id;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
