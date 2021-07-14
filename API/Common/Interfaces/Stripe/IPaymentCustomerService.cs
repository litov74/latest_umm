using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Common.Interfaces.Stripe
{
    public interface IPaymentCustomerService
    {
        Task<bool> AddOrUpdateCustomer(string companyId, string email, string token);
    }
}
