using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.Models.Accounts.Request
{
    public class PlanSubscribeVm
    {
        [MaxLength(254)]
        public string Email { get; set; }
        public string Plan { get; set; }
    }
}
