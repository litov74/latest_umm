using API.Common.Enums;
using API.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Database.Entities
{
    public class CompanyHierarchyItem:BaseStringIdEntity
    {
        public CompnayHierarchyLevel HierarchyLevel { get; set; }
        public string Name { get; set; }
        [MaxLength(36)]
        public string CompanyId { get; set; }
        [MaxLength(36)]
        [Key]
        public Guid? ParentCompanyHierarchyItemId { get; set; }
        public int EmployeeCount { get; set; }

        [ForeignKey("ParentCompanyHierarchyItemId")]
        public CompanyHierarchyItem ParentCompanyHierarchyItem { get; set; }
        [ForeignKey("CompanyId")]
        public CompanyEntity Company { get; set; }
    }
}
