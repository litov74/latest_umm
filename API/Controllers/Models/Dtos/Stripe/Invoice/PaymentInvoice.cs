using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.Models.Dtos.Stripe.Invoice
{
    public class PaymentInvoice
    {
        public string Id { get; set; }
        public bool Paid { get; set; }
        public DateTime? Date { get; set; }
        public string SubscriptionId { get; set; }
        public int AmountDue { get; set; }
        public DateTime PeriodEnd { get; set; }
        public DateTime PeriodStart { get; set; }
        public List<PaymentInvoiceLineItem> InvoiceLineItems { get; set; }
        public bool Closed { get; set; }
    }
}
