using API.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.Models.PlanSubscribe.Request
{
    public class PlanSubscribeVm
    {
        public string Email { get; set; }
        public string SubscriptionTierId { get; set; }
        public string CompanyName { get; set; }
        public int CountUser { get; set; }
    }
}
