using API.Common.Enums;
using API.Common.Interfaces;
using API.Controllers.Models;
using API.Controllers.Models.CompanyHierarchy.Request;
using API.Controllers.Models.Dtos;
using API.Data;
using API.Database.Entities;
using API.Shared;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static API.Shared.ApiFunctions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanySettingsController : ControllerBase
    {
        private readonly ICompanyHierarchyService _companyHierarchyService;
        private readonly APIContext _context;
        private readonly ICompanyService _companyService;
        private readonly IUserClaimService _userClaimService;
        private readonly IMapper _mapper;

        public CompanySettingsController(IMapper mapper, APIContext Context, ICompanyService companyService, ICompanyHierarchyService companyHierarchyService, IUserClaimService userClaimService)
        {
            _companyHierarchyService = companyHierarchyService;
            _companyService = companyService;
            _context = Context;
            _mapper = mapper;
            _userClaimService = userClaimService;
        }
        [HttpGet("GetHierarchyList")]
        public async Task<ApiResponse<List<CompanyHierarchyItemViewModel>>> GetHierarchyList()
        {
            try
            {
                var currentUserCompanyId = _userClaimService.GetCurrentUserCompanyId();
                if (currentUserCompanyId == null)
                {
                    return ApiValidationResponse<List<CompanyHierarchyItemViewModel>>("Company Id Doesn't Exist!");
                }
                var result = _context.CompanyHierarchyItemsEntities.Where(x => x.CompanyId == currentUserCompanyId).ToList();
                if (result == null)
                {
                    return ApiValidationResponse<List<CompanyHierarchyItemViewModel>>("Data Not Found!");
                }
                var dtoMapping = _mapper.Map<List<CompanyHierarchyItemViewModel>>(result);
                foreach (var modelItem in result)
                { 
                    //dtoMapping.ChildrenItemNames = result.Where(x => x.ParentCompanyHierarchyItemId.ToString() == modelItem.Id).Select(x => x.Name).ToList();
                }
                return ApiSuccessResponse(dtoMapping);

            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpGet("GetHierarchyListWithLevels")]
        public async Task<ApiResponse<List<CompanyHierarchyItemsDto>>> GetHierarchyList3()
        {
            var hierarchyList = new List<CompanyHierarchyItem>();
            try
            {
                var currentUserCompanyId = _userClaimService.GetCurrentUserCompanyId();
                if (currentUserCompanyId == null)
                {
                    return ApiValidationResponse<List<CompanyHierarchyItemsDto>>("Company Id Doesn't Exist!");
                }

                var items =  _context.CompanyHierarchyItemsEntities.Where(x => x.CompanyId == currentUserCompanyId).ToList();
                if (items == null)
                {
                    return ApiValidationResponse<List<CompanyHierarchyItemsDto>>("Data Not Found!");
                }
                var division = items.Where(x => x.HierarchyLevel == CompnayHierarchyLevel.Division).ToList();
                var subdivision = items.Where(x => x.HierarchyLevel == CompnayHierarchyLevel.SubDivision).ToList(); 
                var team = items.Where(x => x.HierarchyLevel == CompnayHierarchyLevel.Team).ToList();
                hierarchyList.AddRange(division);
                hierarchyList.AddRange(subdivision);
                hierarchyList.AddRange(team);
                var dtoList = _mapper.Map<List<CompanyHierarchyItemsDto>>(hierarchyList);
                return ApiSuccessResponse(dtoList);

            }
            catch (Exception ex)
            {
                throw;
            }


        }
        [HttpPost("HierarchyListDrilldown")]
        public async Task<ApiResponse<List<CompanyHierarchyItemsDto>>> HierarchyListDrilldown(Guid? parentId)
        {
         
            try
            {
                var currentUserCompanyId = _userClaimService.GetCurrentUserCompanyId();
                if (currentUserCompanyId == null)
                {
                    return ApiValidationResponse<List<CompanyHierarchyItemsDto>>("Company Id Doesn't Exist!");
                }
                else
                {
                    var items =  _context.CompanyHierarchyItemsEntities.Where(x => x.CompanyId == currentUserCompanyId && x.ParentCompanyHierarchyItemId == parentId).ToList();
                    if (items == null)
                    {
                        return ApiValidationResponse<List<CompanyHierarchyItemsDto>>("Data Not Found!");
                    }
                    //var dataList = items.Select(x => new
                    //{
                    //    Name = x.Name,
                    //    Id = x.Id,
                    //    Level = (int)x.HierarchyLevel,
                    //    EmployeeCount = x.EmployeeCount
                    //});
                    var drillList = _mapper.Map<List<CompanyHierarchyItemsDto>>(items);
                    return ApiSuccessResponse(drillList);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }
        [HttpPost("GetHierarchyListMultipleId")]
        public async Task<ApiResponse<List<CompanyHierarchyItemsDto>>> GetHierarchyListMultipleId(List<Guid?> parentGuids)
        {
            
            try
            {
                var currentUserCompanyId = _userClaimService.GetCurrentUserCompanyId();
                if (currentUserCompanyId == null)
                {
                    return ApiValidationResponse<List<CompanyHierarchyItemsDto>>("Company Id Doesn't Exist!");
                }
                var items = _context.CompanyHierarchyItemsEntities.Where(x => x.CompanyId == currentUserCompanyId && parentGuids.Contains(x.ParentCompanyHierarchyItemId)).ToList();
                if (items == null)
                {
                    return ApiValidationResponse<List<CompanyHierarchyItemsDto>>("Data Not Found!");
                }
                var dataList = items.Select(x => new
                {
                    Name = x.Name,
                    Id = x.Id,
                    Level = (int)x.HierarchyLevel,
                    ParentId = x.ParentCompanyHierarchyItemId
                });
                var multipleList = _mapper.Map<List<CompanyHierarchyItemsDto>>(dataList);
                return ApiSuccessResponse(multipleList);

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        [HttpPost("ChangeTolerance")]
        public async Task<ApiResponse<List<CompanyHierarchyItemsDto>>> ChangeTolerance()
        {
            try
            {
                var currentUserCompanyId = _userClaimService.GetCurrentUserCompanyId();
                if (currentUserCompanyId == null)
                {
                    return ApiValidationResponse<List<CompanyHierarchyItemsDto>>("Company Id Doesn't Exist!");
                }
                else
                {
                    var cid = _userClaimService.GetCurrentUserCompanyId();
                    //var model = new CompanySettingsViewModel
                    //{
                    //    CompanyHierarchyItems = _companyService.GetCompanyHierarchyItems(User.Identity.GetCompanyId().Value)
                    //};
                    var data = _context.CompanyHierarchyItemsEntities.Where(x => x.CompanyId == cid).Select(x => new { x.Id, x.ParentCompanyHierarchyItemId, x.Name, x.HierarchyLevel, x.EmployeeCount }).ToList();
                    if (data == null)
                    {
                        return ApiValidationResponse<List<CompanyHierarchyItemsDto>>("Data Not Found!");
                    }
                    var toleranceDto = _mapper.Map<List<CompanyHierarchyItemsDto>>(data);
                    return ApiSuccessResponse(toleranceDto);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }



    }
}
