using System;
using System.Collections.Generic;
using System.Linq;
using API.Common.Enums;
using API.Models.Entities;
using AutoMapper;

namespace API.Models.Dtos
{
    public class UserDto : BaseDto
    {
        public string Email { get; set; }

        public bool IsEmailConfirm { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime CreatedDate { get; set; }

        public List<CompanyUserMappingDto> Companies { get; set; }

        public class CompanyUserMappingDto
        {
            public string Id { get; set; }
            public string CompanyName { get; set; }
            public CompanyAccessLevelEnum AccessLevel { get; set; }
            public bool IsActive { get; set; }
        }

        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<UserEntity, UserDto>()
                    .ForMember(x => x.Companies, ops => ops.MapFrom(m => m.CompanyMappings.Select(cm => new UserDto.CompanyUserMappingDto
                    {
                        Id = cm.CompanyId,
                        CompanyName = cm.Company != null? cm.Company.CompanyName: "",
                        AccessLevel = cm.AccessLevel,
                        IsActive = cm.IsActive
                    })));
            }
        }
    }
}
