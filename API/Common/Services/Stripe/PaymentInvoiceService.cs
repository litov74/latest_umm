using API.Common.Interfaces;
using API.Common.Interfaces.Stripe;
using API.Controllers.Models.Dtos.Stripe.Invoice;
using API.Data;
using AutoMapper;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Common.Services.Stripe
{
    public class PaymentInvoiceService : IPaymentInvoiceService
    {
        private readonly InvoiceService _stripeInvoiceService;
        private readonly IMapper _mapper;
        private readonly APIContext _context;
        private readonly ILogService _logService;
        public PaymentInvoiceService(IMapper mapper, APIContext context, ILogService logService)
        {
            _stripeInvoiceService = new InvoiceService();
            _mapper = mapper;
            _context = context;
            _logService = logService;

        }
        public async Task<List<PaymentInvoice>> GetInvoices()
        {
            var invoices = await _stripeInvoiceService.ListAsync();
            return _mapper.Map<List<PaymentInvoice>>(invoices);
        }
        public async Task<List<PaymentInvoice>> GetInvoicesForCompany(string companyId)
        {
            var companySubscription = await _context.PlanSubscribeEntities.FindAsync(companyId);

            if (companySubscription == null || companySubscription.UserId == null)
                return null;

            var invoices = await _stripeInvoiceService.ListAsync(new InvoiceListOptions
            {
                Customer = companySubscription.UserId
            });

            return _mapper.Map<List<PaymentInvoice>>(invoices);
        }
        public async Task<PaymentInvoice> GetInvoice(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            var invoice = await _stripeInvoiceService.GetAsync(id);

            return _mapper.Map<PaymentInvoice>(invoice);
        }
        public async Task<bool> PayInvoiceAsync(Guid companyId, string invoiceId)
        {
            try
            {
                var companySubscription = await _context.PlanSubscribeEntities.FindAsync(companyId);
                if (companySubscription == null || string.IsNullOrWhiteSpace(companySubscription.UserId))
                {
                    return false;
                }
                var invoice = await _stripeInvoiceService.GetAsync(invoiceId);

                if (invoice == null || companySubscription.UserId != invoice.CustomerId)
                {
                    return false;
                }

                if (invoice.Paid)
                {
                    _logService.LogInfo($"Invoice has been paid: {invoiceId}");
                }
                return await _stripeInvoiceService.PayAsync(invoiceId) != null;
            }
            catch (Exception e)
            {
                _logService.LogError(e, $"Fail pay invoice {invoiceId} Message: {e.Message}");
            }
            return false;
        }
        public async Task<PaymentInvoice> CloseInvoice(string invoiceId)
        {
            var invoice = await _stripeInvoiceService.UpdateAsync(invoiceId, new InvoiceUpdateOptions { AutoAdvance = true });

            return _mapper.Map<PaymentInvoice>(invoice);
        }
    }

}
