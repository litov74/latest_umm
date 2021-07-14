using API.Common.Enums;
using API.Database.Entities;
using API.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.Models.Dtos
{
    public class CompanyHierarchyItemsDto:BaseDto
    {
        public CompnayHierarchyLevel HierarchyLevel { get; set; }
        public string Name { get; set; }
        public int EmployeeCount { get; set; }
        public CompanyHierarchyItemsDto ParentCompanyHierarchyItem { get; set; }
        public CompanyDto Company { get; set; }
    }
}
