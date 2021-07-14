using API.Controllers.Models.Dtos.Stripe.Invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.Models.Dtos
{
    public class PaymentInvoiceReceiptViewModel
    {
        public PaymentInvoice PaymentInvoice { get; set; }
        public PlanSubscribeDto PlanSubscribeDto { get; set; }
    }
}
