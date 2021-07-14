using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.Models.Dtos.Stripe.Invoice
{
    public class PaymentInvoiceLineItem
    {
        public string Id { get; set; }
        public int Amount { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public DateTime? End { get; set; }
        public DateTime? Start { get; set; }
        public PaymentPlan Plan { get; set; }
    }
}
