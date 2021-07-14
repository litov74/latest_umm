using API.Common.Enums;
using API.Database.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.Models
{
    public class CompanyHierarchyItemVm
    {
        public string Id { get; set; }
        public CompnayHierarchyLevel HierarchyLevel { get; set; }
        public string Name { get; set; }
        public int EmployeeCount { get; set; }
        public string CompanyId { get; set; }
        public Guid? CompanyHierarchyItemId { get; set; }
    }
}
