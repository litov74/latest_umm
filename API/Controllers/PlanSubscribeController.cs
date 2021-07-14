using API.Common.Enums;
using API.Common.Interfaces;
using API.Common.Models;
using API.Controllers.Models.Dtos;
using API.Controllers.Models.PlanSubscribe.Request;
using API.Shared;
using Microsoft.AspNetCore.Authorization;
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
    public class PlanSubscribeController : ControllerBase
    {
        private readonly IPlanSubscribeService _planSubscribeService;
        public PlanSubscribeController(IPlanSubscribeService planSubscribeService)
        {
            _planSubscribeService = planSubscribeService;
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseJson<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseJson<string>))]
        [Route("[action]")]
        public async Task<ActionResult<ApiResponse<string>>> PlanSubscribe(PlanSubscribeVm planSubscribeVm)
        {
            return await _planSubscribeService.CreatePlanSubscribe(planSubscribeVm);
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseJson<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseJson<string>))]
        [Route("[action]")]
        public async Task<ActionResult<ApiResponse<List<PlanSubscribeDto>>>> GetAllPlanSubscribeUserHistory()
        {
            return await _planSubscribeService.GetAllPlanSubscribeUserHistory();
        }
    }
}
