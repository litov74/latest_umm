using API.Controllers.Models;
using API.Controllers.Models.Dtos;
using API.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Common.Interfaces
{
    public interface ICompanyHierarchyService
    {
        Task<ApiResponse<CompanyHierarchyItemsDto>> CreateCompanyHierarchy(CompanyHierarchyItemVm companyHierarchyItemVm);
        Task<ApiResponse<CompanyHierarchyItemsDto>> UpdateCompanyHierarchy(CompanyHierarchyItemVm companyHierarchyItemVm);
        Task<ApiResponse<List<CompanyHierarchyItemsDto>>> GetAllCompanyHierarchy();
        Task<ApiResponse<bool>> DeleteCompanyHierarchyById(string Id);
        Task<ApiResponse<CompanyHierarchyItemsDto>> GetCompanyHierarchyById(string Id);
        Task<ApiResponse<List<CompanyHierarchyItemsDto>>> GetCompanyHierarchyItemsByCompanyId(string CompanyId);
        Task<ApiResponse<List<CompanyHierarchyItemsDto>>> GetCompanyHierarchyItemsSubTree(string nodeItemId);
        Task<ApiResponse<List<CompanyHierarchyItemsDto>>> GetCompanyHierarchyItems(string companyId, string parentId);
        Task<ApiResponse<List<CompanyHierarchyItemsDto>>> GetParantCompanyHierarchyItems(string companyId, List<string> parentId);
        Task<ApiResponse<List<CompanyHierarchyItemsDto>>> GetByIds(List<string> id);
    }
}
