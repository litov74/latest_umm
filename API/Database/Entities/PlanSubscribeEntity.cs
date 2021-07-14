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
    public class PlanSubscribeEntity : BaseStringIdEntity
    {
        [MaxLength(36)]
        public string UserId { get; set; }
        [MaxLength(36)]
        public string CompanyId { get; set; }
        [MaxLength(36)]
        public string SubscriptionTierId { get; set; }
        public string Plan { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public int CountUser { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ExpiryDate { get; set; }
        public bool IsPaymentVerified { get; set; }
        [ForeignKey("UserId")]
        public UserEntity User { get; set; }
        [ForeignKey("SubscriptionTierId")]
        public SubscriptionTierEntity SubscriptionTier { get; set; }
        [ForeignKey("CompanyId")]
        public CompanyEntity Company { get; set; }

    }
}
