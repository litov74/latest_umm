using API.Common.Interfaces;
using API.Controllers.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Common.Services
{
    public class EnumValueService : IEnumValueService
    {
        public async Task<List<EnumValues>> GetAllCompnaySubscriptionPlanEnum<T>()
        {
            List<EnumValues> values = new List<EnumValues>();
            foreach (var itemType in await Task.FromResult(Enum.GetValues(typeof(T))))
            {
                values.Add(new EnumValues()
                {
                    Name = Enum.GetName(typeof(T), itemType),
                    Value = (int)itemType
                });
            }
            return values;
        }
        public async Task<List<EnumValues>> GetAllCompanyAccessLevels<T>()
        {
            List<EnumValues> values = new List<EnumValues>();
            foreach (var itemType in await Task.FromResult(Enum.GetValues(typeof(T))))
            {
                values.Add(new EnumValues()
                {
                    Name = Enum.GetName(typeof(T), itemType),
                    Value = (int)itemType
                });
            }
            return values;
        }
        public async Task<List<EnumValues>> GetAllCompanyAccessTypes<T>()
        {
            List<EnumValues> values = new List<EnumValues>();
            foreach (var itemType in await Task.FromResult(Enum.GetValues(typeof(T))))
            {
                values.Add(new EnumValues()
                {
                    Name = Enum.GetName(typeof(T), itemType),
                    Value = (int)itemType
                });
            }
            return values;
        }
        public async Task<List<EnumValues>> GetAllCompanyHierarchyLevels<T>()
        {
            List<EnumValues> values = new List<EnumValues>();
            foreach (var itemType in await Task.FromResult(Enum.GetValues(typeof(T))))
            {
                values.Add(new EnumValues()
                {
                    Name = Enum.GetName(typeof(T), itemType),
                    Value = (int)itemType
                });
            }
            return values;
        }
    }
}
