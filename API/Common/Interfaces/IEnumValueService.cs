using API.Controllers.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Common.Interfaces
{
    public interface IEnumValueService
    {
        Task<List<EnumValues>> GetAllCompnaySubscriptionPlanEnum<T>();
        Task<List<EnumValues>> GetAllCompanyAccessLevels<T>();
        Task<List<EnumValues>> GetAllCompanyAccessTypes<T>();
        Task<List<EnumValues>> GetAllCompanyHierarchyLevels<T>();

    }
}
