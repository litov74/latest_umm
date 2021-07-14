using API.Common.Enums;
using API.Common.Interfaces;
using API.Controllers.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static API.Common.Enums.CompanyAccessType;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonEnumValuesController : ControllerBase
    {
        private readonly IEnumValueService _enumValueService;
        public CommonEnumValuesController(IEnumValueService enumValueService)
        {
            _enumValueService = enumValueService;
        }
        [HttpGet("GetAllSubscriptionPlan")]
        public async Task<ActionResult<List<EnumValues>>> GetAllSubscriptionPlan()
        {
            return await _enumValueService.GetAllCompnaySubscriptionPlanEnum<CompanySubscriptionPlanEnum>();
        }
        [HttpGet("GetAllCompanyAccessLevels")]
        public async Task<ActionResult<List<EnumValues>>> GetAllCompanyAccessLevels()
        {
            return await _enumValueService.GetAllCompanyAccessLevels<CompanyAccessLevelEnum>();
        }
        [HttpGet("GetAllCompanyAccessTypes")]
        public async Task<ActionResult<List<EnumValues>>> GetAllCompanyAccessTypes()
        {
            return await _enumValueService.GetAllCompanyAccessTypes<CompanyAccessTypes>();
        }
        [HttpGet("GetAllCompanyHierarchyLevels")]
        public async Task<ActionResult<List<EnumValues>>> GetAllCompanyHierarchyLevels()
        {
            return await _enumValueService.GetAllCompanyHierarchyLevels<CompnayHierarchyLevel>();
        }

    }
}
