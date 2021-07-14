using API.Controllers.Models;
using API.Controllers.Models.Company;
using API.Controllers.Models.CompanyHierarchy.Request;
using API.Controllers.Models.Dtos;
using API.Controllers.Models.Dtos.Stripe.Invoice;
using API.Database.Entities;
using API.Models.Dtos;
using API.Models.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Common.Helppers
{
    public class ProfilesMapping : Profile
    {
        public ProfilesMapping()
        {
            CreateMap<SubscriptionTierEntity, SubscriptionTierDto>();

            CreateMap<CompanyHierarchyItem, CompanyHierarchyItemsDto>().ReverseMap();
            CreateMap<CompanyHierarchyItemVm, CompanyHierarchyItem>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            //CreateMap<CompanyHierarchyItem, CompanyHierarchyItemViewModel>().ForMember(dest => dest.AllHierarchyItems,opt => opt.MapFrom(src => src.HierarchyLevel + " " + src.Name + " " + src.CompanyId + " " + src.ParentCompanyHierarchyItemId + " " + src.EmployeeCount + " " + src.ParentCompanyHierarchyItem + " " + src.Company));
            CreateMap<CompanyHierarchyItem, CompanyHierarchyItemViewModel>().ReverseMap();
            //CreateMap<CompanyHierarchyItem, CompanyHierarchyItemViewModel>().ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.CompanyId))
            //                                                                .ForMember(dest => dest.HierarchyLevel, opt => opt.MapFrom(src => src.HierarchyLevel))
            //                                                                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            //                                                                .ForMember(dest => dest.ParentCompanyHierarchyItemId, opt => opt.MapFrom(src => src.ParentCompanyHierarchyItemId))
            //                                                                .ForMember(dest => dest.EmployeeCount, opt => opt.MapFrom(src => src.EmployeeCount))
            //                                                                //.ForMember(dest => dest.AllHierarchyItems, opt => opt.MapFrom(src => src.HierarchyLevel + " " + src.Name + " " + src.CompanyId + " " + src.ParentCompanyHierarchyItemId + " " + src.EmployeeCount))
            //                                                                .ReverseMap();
                                                                            

            CreateMap<CompanyEntity, CompanyDto>().ReverseMap();
            CreateMap<PlanSubscribeEntity, PaymentInvoice>();
            CreateMap<PlanSubscribeEntity, PlanSubscribeDto>().ReverseMap();
            CreateMap<CompanyLoadingLevelEntity, CompanyLoadingLevelDto>().ReverseMap();

            CreateMap<CompanyDto, CompanyEditViewModel>().ReverseMap();
            CreateMap<CompanySubscriptionDto, CompanySubscriptionViewModel>().ReverseMap();
        }
    }
}
