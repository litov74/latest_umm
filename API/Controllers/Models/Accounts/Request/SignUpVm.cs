using System.ComponentModel.DataAnnotations;
using API.Common.Enums;
using AutoMapper;

namespace API.Controllers.Models.Accounts.Request
{
    public class SignUpVm
    {
        [Required]
        [StringLength(254)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string Password { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string CompanyName { get; set; }

        [Required]
        [EnumDataType(typeof(CompanySubscriptionPlanEnum))]
        public string Plan { get; set; }
        public CompanyAccessLevelEnum AccessLevels { get; set; }

        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                //CreateMap<SignUpVm, UserEntity>();
            }
        }
    }
}
