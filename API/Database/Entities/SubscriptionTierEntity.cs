using API.Common.Enums;
using API.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Database.Entities
{
    public class SubscriptionTierEntity:BaseStringIdEntity
    {
        
        [Required]
        public CompanySubscriptionPlanEnum TierName { get; set; }
        [Column(TypeName ="decimal(10,2)")]
        [Required]
        public decimal Price { get; set; }
        public int NumberOfEmployees { get; set; }
        [DataType(DataType.Date)]
        public DateTime Monthly { get; set; }
        [DataType(DataType.Date)]
        public DateTime Annually { get; set; }
        public bool CCAccess { get; set; }
        public bool CAAccess { get; set; }
    }
}
