using API.Common.Interfaces;
using API.Common.Models;
using API.Controllers.Models;
using API.Controllers.Models.Dtos;
using API.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompnayHierarchyItemsController : ControllerBase
    {
        private readonly ICompanyHierarchyService _companyHierarchyService;
        public CompnayHierarchyItemsController(ICompanyHierarchyService companyHierarchyService)
        {
            _companyHierarchyService = companyHierarchyService;
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseJson<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseJson<string>))]
        [Route("[action]")]
        public async Task<ActionResult<ApiResponse<CompanyHierarchyItemsDto>>> CreateCompanyHierarchy(CompanyHierarchyItemVm companyHierarchyItemVm)
        {
            return await _companyHierarchyService.CreateCompanyHierarchy(companyHierarchyItemVm);
        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseJson<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseJson<string>))]
        [Route("[action]")]
        public async Task<ActionResult<ApiResponse<CompanyHierarchyItemsDto>>> UpdateCompanyHierarchy(CompanyHierarchyItemVm companyHierarchyItemVm)
        {
            return await _companyHierarchyService.UpdateCompanyHierarchy(companyHierarchyItemVm);
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseJson<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseJson<string>))]
        [Route("[action]")]
        public async Task<ActionResult<ApiResponse<List<CompanyHierarchyItemsDto>>>> GetAllCompanyHierarchy()
        {
            return await _companyHierarchyService.GetAllCompanyHierarchy();
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseJson<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseJson<string>))]
        [Route("[action]")]
        public async Task<ActionResult<ApiResponse<CompanyHierarchyItemsDto>>> GetCompanyHierarchyById(string Id)
        {
            return await _companyHierarchyService.GetCompanyHierarchyById(Id);
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseJson<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseJson<string>))]
        [Route("[action]")]
        public async Task<ActionResult<ApiResponse<List<CompanyHierarchyItemsDto>>>> GetCompanyHierarchyItemsSubTree(string nodeItemId)
        {
            return await _companyHierarchyService.GetCompanyHierarchyItemsSubTree(nodeItemId);
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseJson<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseJson<string>))]
        [Route("[action]")]
        public async Task<ActionResult<ApiResponse<List<CompanyHierarchyItemsDto>>>> GetCompanyHierarchyItems(string companyId, string parentId)
        {
            return await _companyHierarchyService.GetCompanyHierarchyItems(companyId, parentId);
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseJson<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseJson<string>))]
        [Route("[action]")]
        public async Task<ActionResult<ApiResponse<List<CompanyHierarchyItemsDto>>>> GetCompanyHierarchyItemsByCompanyId(string companyId)
        {
            return await _companyHierarchyService.GetCompanyHierarchyItemsByCompanyId(companyId);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseJson<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseJson<string>))]
        [Route("[action]")]
        public async Task<ActionResult<ApiResponse<List<CompanyHierarchyItemsDto>>>> GetParantCompanyHierarchyItems(string companyId, List<string> parentId)
        {
            return await _companyHierarchyService.GetParantCompanyHierarchyItems(companyId, parentId);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseJson<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseJson<string>))]
        [Route("[action]")]
        public async Task<ActionResult<ApiResponse<List<CompanyHierarchyItemsDto>>>> GetByIds(List<string> id)
        {
            return await _companyHierarchyService.GetByIds(id);
        }
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseJson<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseJson<string>))]
        [Route("[action]")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteCompanyHierarchyById(string Id)
        {
            return await _companyHierarchyService.DeleteCompanyHierarchyById(Id);
        }
    }
}
