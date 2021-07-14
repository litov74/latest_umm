using System;
using System.ComponentModel.DataAnnotations;

namespace API.Models.Entities
{
    public abstract class BaseTrackingEntity
    {
        public DateTime CreatedAt { get; set; }
        [MaxLength(254)]
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [MaxLength(254)]
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

    }
}
