using API.Controllers.Models.Dtos.Stripe.Invoice;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.Models.Dtos.Stripe
{
    public class PlanSelectionViewModel
    {
        [Required(ErrorMessage = "The maintenance period field is required.")]
        public string PlanId { get; set; }
        public bool UseFreeTrial { get; set; }
        public bool CanUseTrial { get; set; }
        public string stripeEmail { get; set; }
        public string stripeToken { get; set; }
        public string stripeCouponCode { get; set; }
        public List<PaymentPlan> PaymentListsModel { get; set; }
    }
    public class PlanViewerSelectionModel
    {
        public string PlanId { get; set; }
        public string StripeToken { get; set; }
        public string StripeCouponCode { get; set; }
    }
}
