using API.Common.Enums;
using API.Common.Interfaces;
using API.Controllers;
using API.Controllers.Models.Dtos;
using API.Data;
using API.Database.Entities;
using API.Models.Dtos;
using API.Models.Entities;
using API.Shared;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static API.Shared.ApiFunctions;

namespace API.Common.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ILogService _logger;
        private readonly APIContext _context;
        private readonly IMapper _mapper;
        private readonly ICompanyHierarchyService _companyHierarchyService;
        Dictionary<string, string> additionalClaims = new Dictionary<string, string>();

        public CompanyService(
           ILogService logger,
           APIContext context,
           IMapper mapper,
           ICompanyHierarchyService companyHierarchyService)

        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _companyHierarchyService = companyHierarchyService;
        }
        public async Task<ApiResponse<CompanyDto>> GetCompanyById(string id)
        {
            var findEntry = await _context.CompanyEntity.FindAsync(id);
            if (findEntry == null)
            {
                return ApiValidationResponse<CompanyDto>("Company Not Found!");
            }
            var companyDto = _mapper.Map<CompanyDto>(findEntry);
            return ApiSuccessResponse(companyDto);
        }

        public void AddCompany(CompanyEntity company, string adminEmail, string adminPassword = null)
        {
            Task.FromResult(_context.SaveChangesAsync());
            CreateCompanyEntities(company, adminEmail, adminPassword);
        }
        public async Task<ApiResponse<CompanyDto>> UpdateCompany(CompanyDto company)
        {
            var findEntry = await _context.CompanyEntity.FirstOrDefaultAsync(x => x.Id == company.Id);
            if (findEntry == null)
            {
                return ApiValidationResponse<CompanyDto>("CompanyId Missmatch! Record Not Found!");
            }
            var updateEntry = _mapper.Map<CompanyDto, CompanyEntity>(company, findEntry);
            updateEntry.UpdatedAt = DateTime.UtcNow;
             await _context.SaveChangesAsync();
            var dtoUpdatedList = _mapper.Map<CompanyDto>(updateEntry);
            return ApiSuccessResponse(dtoUpdatedList);
        }
        public void AddUpdateCompanyLoadingLevel(CompanyLoadingLevelEntity loadingLevel)
        {
            var rep = _context.CompanyLoadingLevelEntities;

            if (rep.ToList().Any(x => x.Week == loadingLevel.Week && x.CompanyId == loadingLevel.CompanyId && x.HierarchyItemId == loadingLevel.HierarchyItemId))
            {
                var a = rep.ToList().Where(x => x.Week == loadingLevel.Week && x.CompanyId == loadingLevel.CompanyId && x.HierarchyItemId == loadingLevel.HierarchyItemId)
                    .First();
                a.Level = loadingLevel.Level;
                a.From = loadingLevel.From;
                a.To = loadingLevel.To;
                rep.Update(a);
            }
            else
            {
                rep.Add(new CompanyLoadingLevelEntity()
                {
                    CompanyId = loadingLevel.CompanyId,
                    HierarchyItemId = loadingLevel.HierarchyItemId,
                    Level = loadingLevel.Level,
                    Week = loadingLevel.Week,
                    From = loadingLevel.From,
                    To = loadingLevel.To
                });
            }

            Task.FromResult(_context.SaveChangesAsync());
        }
        public async void CreateCompanyEntities(CompanyEntity company, string adminEmail, string adminPassword = null)
        {
            var companyId = company.Id;
            var companyAdmin = new UserEntity
            {
                FirstName = "Global",
                LastName = "Admin",
                Email = adminEmail,
                Password = string.IsNullOrWhiteSpace(adminPassword) ? "Admin@99" : adminPassword,
                CreatedAt = DateTime.UtcNow,
            };
            var comapnyDetails = new CompanyEntity
            {
                Id = companyId,
                CompanyName = company.CompanyName,
                CreatedAt = DateTime.UtcNow,
                IsEnabled = company.IsEnabled,
                AccessType = company.AccessType
            };
            var CompanyUserMapping = new CompanyUserMappingEntity
            {
                CompanyId = comapnyDetails.Id,
                UserId = companyAdmin.Id,
                AccessLevel = CompanyAccessLevelEnum.Admin
            };
            additionalClaims.Add(ClaimTypes.Role, Roles.GlobalAdmin);
            await _context.AddAsync(companyAdmin);
            await _context.AddAsync(comapnyDetails);
            await _context.AddAsync(CompanyUserMapping);
            try
            {
                //var result = new UsersController().PostCreate(companyAdmin);
                //_userManager.Create(companyAdmin, password);
                //_userManager.AddToRole(companyAdmin.Id, UserRoles.CompanyAdmin);

                //CreateChangeTypeEntities(companyId);
                //CreateInitiativePhaseEntities(companyId);
                //CreateInitiativeStrategicAlignmentEntities(companyId);

                //CreateCompanyBenefitDetails(companyId);
                //CreateCompanyBenefitHealthEntities(companyId);

                //CreateImpactActivityEntities(companyId);
                //CreateImpactCustomerTypeEntities(companyId);
                //CreateImpactGeographyEntities(companyId);
                //CreateImpactScaleEntities(companyId);
                //CreateStakeholderEntities(companyId);
                //CreateNumberCustomerScaleEntities(companyId);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
        public async void AddOrUpdateCompanySettings(CompanySettingsEnum settingsEnum, string companyId, string value, bool withImpactlvl0)
        {
            var settings = _context.CompanySettingsEntities.FirstOrDefault(x => x.CompanyId == companyId && x.Key == settingsEnum);
            if (settings != null)
            {
                settings.Value = value;
                settings.WithImpactlvl0 = withImpactlvl0;
                await _context.SaveChangesAsync();
            }
            else
            {
                settings = new CompanySettingsEntity()
                {
                    CompanyId = companyId,
                    Key = settingsEnum,
                    Value = value,
                    WithImpactlvl0 = withImpactlvl0
                };
                await _context.CompanySettingsEntities.AddAsync(settings);
            }
            await _context.SaveChangesAsync();
        }
        public string GetCompanySettings(CompanySettingsEnum settingsEnum, string companyId)
        {
            var getCompanySettings = _context.CompanySettingsEntities.Where(x => x.CompanyId == companyId && x.Key == settingsEnum).FirstOrDefault().Value;
            return getCompanySettings;
        }
        public async Task<ApiResponse<PlanSubscribeDto>> AddCompanySubscription(PlanSubscribeDto subscription)
        {
            var subscribeEntity = _mapper.Map<PlanSubscribeEntity>(subscription);
            subscribeEntity.CreatedAt = DateTime.UtcNow;
            var addEntry = _context.PlanSubscribeEntities.AddAsync(subscribeEntity);
            await _context.SaveChangesAsync();
            var subscribeDto = _mapper.Map<PlanSubscribeDto>(addEntry);
            return ApiSuccessResponse(subscribeDto);
        }
        public List<string> GetSubdivisions(Guid dId)
        {
            return _context.CompanyHierarchyItemsEntities.Where(x => x.ParentCompanyHierarchyItemId == dId && x.HierarchyLevel == CompnayHierarchyLevel.SubDivision)
                .Select(x => x.Id)
                .ToList();
        }
        public int GetCompanyCount(string active_inActive)
        {
            int compCount = 0;
            if (active_inActive == "total")
            {
                compCount = _context.CompanyEntity.ToList().Count();
            }
            else if (active_inActive == "active")
            {
                compCount = _context.CompanyEntity.ToList().Count(c => c.IsEnabled == true);
            }
            else if (active_inActive == "inActive")
            {
                compCount = _context.CompanyEntity.ToList().Count(c => c.IsEnabled == false);
            }
            else if (active_inActive == "accessType")
            {
                compCount = _context.CompanyEntity.ToList().Count(c => c.AccessType == 0);
            }
            return compCount;
        }
        public int GetSubscriptionCount(string Free_Paid)
        {
            int Count = 0;
            if (Free_Paid == "free")
            {
                Count = _context.PlanSubscribeEntities.ToList().Count(x => x.SubscriptionTierId == null);
            }
            else if (Free_Paid == "paid")
            {
                Count = _context.PlanSubscribeEntities.ToList().Count(x => x.SubscriptionTierId != null);
            }
            return Count;
        }
        public int GetCompanyUserCount(string copmanyId)
        {
            var countList = _context.CompanyUserMappingEntity.Where(x => x.CompanyId == copmanyId).Count();
            return countList;
        }
        public string GetCompanyOwnerEmail(string copmanyId)
        {
            var getEmail = _context.CompanyUserMappingEntity.Where(x => x.CompanyId == copmanyId && x.User.IsBaseUserOfCompany).Select(u => new
            {
                u.User.Email
            }).FirstOrDefault();
            return getEmail.Email;
        }

    }
}
