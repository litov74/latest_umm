using API.Common.Enums;
using API.Common.Interfaces;
using API.Common.Models;
using API.Controllers.Models.Dtos;
using API.Controllers.Models.SubscriptionTier.Request;
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
    [Authorize]
    [ApiController]
    public class SubscriptionTierController : ControllerBase
    {
        private readonly ISubscriptionTierService _subscriptionTierService;
        public SubscriptionTierController(ISubscriptionTierService subscriptionTierService)
        {
            _subscriptionTierService = subscriptionTierService;
        }
       
        [HttpPost]
        [Authorize(Roles = Roles.GlobalAdmin)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseJson<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseJson<string>))]
        [Route("[action]")]
        public async Task<ActionResult<ApiResponse<string>>> CreateSubscriptionTier(SubscriptionTierVm subscriptionTierVm)
        {
            return await _subscriptionTierService.CreateSubscriptionTier(subscriptionTierVm);
        }
        [HttpGet]
        [Authorize(Roles = Roles.GlobalAdmin)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseJson<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseJson<string>))]
        [Route("[action]")]
        public async Task<ActionResult<ApiResponse<List<SubscriptionTierDto>>>> GetAllSubscriptionTier()
        {
            return await _subscriptionTierService.GetAllSubscriptionTier();
        }
        [HttpGet]
        [Authorize(Roles = Roles.GlobalAdmin)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseJson<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseJson<string>))]
        [Route("[action]")]
        public async Task<ActionResult<ApiResponse<SubscriptionTierDto>>> GetSubscriptionTierById(string Id)
        {
            return await _subscriptionTierService.GetSubscriptionTierById(Id);
        }
        [HttpPut]
        [Authorize(Roles = Roles.GlobalAdmin)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseJson<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseJson<string>))]
        [Route("[action]")]
        public async Task<ActionResult<ApiResponse<string>>> UpdateSubscriptionTier(SubscriptionTierVm subscriptionTierVm)
        {
            return await _subscriptionTierService.UpdateSubscriptionTier(subscriptionTierVm);
        }
        [HttpDelete]
        [Authorize(Roles = Roles.GlobalAdmin)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseJson<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseJson<string>))]
        [Route("[action]")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteSubscriptionTier(string Id)
        {
            return await _subscriptionTierService.DeleteSubscriptionTier(Id);
        }


    }
}
