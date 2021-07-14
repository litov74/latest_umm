using API.Common.Enums;
using API.Controllers.Models.Dtos;
using API.Models.Dtos;
using API.Models.Entities;
using API.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Common.Interfaces
{
    public interface ICompanyService
    {
        Task<ApiResponse<CompanyDto>> GetCompanyById(string id);
        void AddCompany(CompanyEntity company, string adminEmail, string adminPassword = null);
        void CreateCompanyEntities(CompanyEntity company, string adminEmail, string adminPassword = null);
        void AddOrUpdateCompanySettings(CompanySettingsEnum settingsEnum, string companyId, string value, bool withImpactlvl0);
        string GetCompanySettings(CompanySettingsEnum settingsEnum, string companyId);
        Task<ApiResponse<CompanyDto>> UpdateCompany(CompanyDto company);
        Task<ApiResponse<PlanSubscribeDto>> AddCompanySubscription(PlanSubscribeDto subscription);
        List<string> GetSubdivisions(Guid dId);
        int GetCompanyCount(string active_inActive);
        int GetSubscriptionCount(string Free_Paid);
    }
}
