using API.Controllers.Models.Dtos.Stripe.Invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Common.Interfaces.Stripe
{
    public interface IPaymentInvoiceService
    {
        Task<List<PaymentInvoice>> GetInvoices();
        Task<List<PaymentInvoice>> GetInvoicesForCompany(string companyId);
        Task<PaymentInvoice> GetInvoice(string id);
        Task<bool> PayInvoiceAsync(Guid companyId, string invoiceId);
        Task<PaymentInvoice> CloseInvoice(string invoiceId);
    }
}
