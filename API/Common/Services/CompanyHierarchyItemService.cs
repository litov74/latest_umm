using API.Common.Interfaces;
using API.Controllers.Models;
using API.Controllers.Models.Dtos;
using API.Data;
using API.Database.Entities;
using API.Shared;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static API.Shared.ApiFunctions;

namespace API.Common.Services
{
    public class CompanyHierarchyItemService : ICompanyHierarchyService
    {
        private readonly APIContext _context;
        private readonly IMapper _mapper;
        public CompanyHierarchyItemService(APIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<CompanyHierarchyItemsDto>> CreateCompanyHierarchy(CompanyHierarchyItemVm companyHierarchyItemVm)
        {
            try
            {
                var findCompany = await _context.CompanyEntity.FirstOrDefaultAsync(x => x.Id == companyHierarchyItemVm.CompanyId && x.CompanyName == companyHierarchyItemVm.Name.ToLower().Trim());
                if (findCompany == null)
                {
                    return ApiErrorResponse<CompanyHierarchyItemsDto>("Company Name Not Found!");
                }
                var hierarchyEntity = _mapper.Map<CompanyHierarchyItem>(companyHierarchyItemVm);
                hierarchyEntity.CreatedAt = DateTime.UtcNow;
                await _context.CompanyHierarchyItemsEntities.AddAsync(hierarchyEntity);
                await _context.SaveChangesAsync();
                var hierarchyDto = _mapper.Map<CompanyHierarchyItemsDto>(hierarchyEntity);
                return ApiSuccessResponse(hierarchyDto);
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public async Task<ApiResponse<CompanyHierarchyItemsDto>> UpdateCompanyHierarchy(CompanyHierarchyItemVm companyHierarchyItemVm)
        {
            var findEntry = await _context.CompanyHierarchyItemsEntities.FirstOrDefaultAsync(x => x.Id == companyHierarchyItemVm.Id);
            if (findEntry == null)
            {
                return ApiErrorResponse<CompanyHierarchyItemsDto>("Record Not Found!");
            }
            var hierarchyEntity = _mapper.Map<CompanyHierarchyItemVm, CompanyHierarchyItem>(companyHierarchyItemVm, findEntry);
            hierarchyEntity.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            var hierarchyDto = _mapper.Map<CompanyHierarchyItemsDto>(hierarchyEntity);
            return ApiSuccessResponse(hierarchyDto);
        }
        public async Task<ApiResponse<List<CompanyHierarchyItemsDto>>> GetAllCompanyHierarchy()
        {
            var getList = await _context.CompanyHierarchyItemsEntities.ToListAsync();
            var dtoList = _mapper.Map<List<CompanyHierarchyItemsDto>>(getList);
            return ApiSuccessResponse(dtoList);
        }

        public async Task<ApiResponse<bool>> DeleteCompanyHierarchyById(string id)
        {
            var findEntry = await _context.CompanyHierarchyItemsEntities.FirstOrDefaultAsync(x => x.Id == id);
            if (findEntry == null)
            {
                return ApiErrorResponse<bool>("Record Not Found!");
            }
            _context.CompanyHierarchyItemsEntities.Remove(findEntry);
            await _context.SaveChangesAsync();
            return ApiSuccessResponse(true);

        }
        public async Task<ApiResponse<CompanyHierarchyItemsDto>> GetCompanyHierarchyById(string Id)
        {
            var getRecord = await _context.CompanyHierarchyItemsEntities.FirstOrDefaultAsync(x => x.Id == Id);
            var dtoList = _mapper.Map<CompanyHierarchyItemsDto>(getRecord);
            return ApiSuccessResponse(dtoList);
        }
        public async Task<ApiResponse<List<CompanyHierarchyItemsDto>>> GetCompanyHierarchyItemsByCompanyId(string companyId)
        {
            var getList= await _context.CompanyHierarchyItemsEntities.Where(x => x.CompanyId == companyId).ToListAsync();
            var dtoList = _mapper.Map<List<CompanyHierarchyItemsDto>>(getList);
            return ApiSuccessResponse(dtoList);
        }
        public async Task<ApiResponse<List<CompanyHierarchyItemsDto>>> GetCompanyHierarchyItemsSubTree(string nodeItemId)
        {
            var result = await _context.CompanyHierarchyItemsEntities.Where(x => x.Id == nodeItemId || x.ParentCompanyHierarchyItemId.ToString() == nodeItemId).ToListAsync();
            var hiearachyDto = _mapper.Map<List<CompanyHierarchyItemsDto>>(result);
            return ApiSuccessResponse(hiearachyDto);

        }
        public async Task<ApiResponse<List<CompanyHierarchyItemsDto>>> GetCompanyHierarchyItems(string companyId, string parentId)
        {
            var findEntry = await _context.CompanyHierarchyItemsEntities.Where(x => x.CompanyId == companyId && x.ParentCompanyHierarchyItemId.ToString() == parentId).ToListAsync();
            var hirerachyDtos = _mapper.Map<List<CompanyHierarchyItemsDto>>(findEntry);
            return ApiSuccessResponse(hirerachyDtos);
        }
        public async Task<ApiResponse<List<CompanyHierarchyItemsDto>>> GetParantCompanyHierarchyItems(string companyId, List<string> parentId)
        {
            var getAllId = parentId;
            var getList =await _context.CompanyHierarchyItemsEntities.Where(x => x.CompanyId == companyId && parentId.Contains(x.ParentCompanyHierarchyItemId.ToString())).ToListAsync();
            var dtoList = _mapper.Map<List<CompanyHierarchyItemsDto>>(getList);
            return ApiSuccessResponse(dtoList);
        }
        public async Task<ApiResponse<List<CompanyHierarchyItemsDto>>> GetByIds(List<string> id)
        {
            var findItems = await _context.CompanyHierarchyItemsEntities.Where(x => id.Contains(x.Id)).ToListAsync();
            var itemsDto = _mapper.Map<List<CompanyHierarchyItemsDto>>(findItems);
            return ApiSuccessResponse(itemsDto);
        }
    }
}
