using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using API.Common.Enums;
using API.Models.Entities;
using AutoMapper;

namespace API.Controllers.Models.Users.Request
{
    public class UserCreateVm
    {
        [Required]
        [StringLength(254)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        public bool IsEmailConfirm { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public List<CompanyUserMappingCreateVm> Companies { get; set; }


        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<UserCreateVm, UserEntity>()
                    .ForMember(x => x.CompanyMappings, ops => ops.MapFrom(m => m.Companies.Select(cm => new CompanyUserMappingEntity
                    {
                        CompanyId = cm.Id,
                        AccessLevel = cm.AccessLevel,
                        IsActive = cm.IsActive
                    })))
                    .ForMember(x => x.Password, ops => ops.Ignore());
            }
        }
    }
}
