
using API.Controllers.Models.Dtos;
using API.Controllers.Models.PlanSubscribe.Request;
using API.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Common.Interfaces
{
    public interface IPlanSubscribeService
    {
        Task<ApiResponse<string>> CreatePlanSubscribe(PlanSubscribeVm planSubscribeVm);
        Task<ApiResponse<List<PlanSubscribeDto>>> GetAllPlanSubscribeUserHistory();
    }
}
