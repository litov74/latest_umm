using System.ComponentModel.DataAnnotations;
using API.Common.Enums;

namespace API.Models.Entities
{
    public class CompanyUserMappingEntity : BaseTrackingEntity
    {
        [Key]
        [MaxLength(36)]
        public string CompanyId { get; set; }
        [Key]
        [MaxLength(36)]
        public string UserId { get; set; }
        public CompanyAccessLevelEnum AccessLevel { get; set; }
        public bool IsActive { get; set; }

        public virtual CompanyEntity Company { get; set; }
        public virtual UserEntity User { get; set; }
    }
}
