using API.Common.Enums;
using API.Database.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.Models.CompanyHierarchy.Request
{
    public class CompanyHierarchyItemViewModel
    {
        [DisplayName("Level")]
        public CompnayHierarchyLevel HierarchyLevel { get; set; }

        [DisplayName("Name")]
        [Required]
        public string Name { get; set; }

        public Guid CompanyId { get; set; }
        public Guid Id { get; set; }
        public Guid? ParentCompanyHierarchyItemId { get; set; }
        public int EmployeeCount { get; set; }

        [DisplayName("Children Items")]
        public List<Guid> ChildrenItemsIds { get; set; }
        public List<string> ChildrenItemNames { get; set; }

        public bool IsCategory { get; set; }

        public List<CompanyHierarchyItem> AllHierarchyItems { get; set; }
        public List<CompanyHierarchyItem> PossibleParentHierarchyItems { get; set; }

        public bool HasChildren { get; set; }
        public bool IsUsedInInitiatives { get; set; }

        [Display(Name = "Move to")]
        public Guid? MoveToId { get; set; }
        public IEnumerable<CompanyHierarchyItemViewModel> MoveToList { get; set; }

        public CompanyHierarchyItemViewModel()
        {
            MoveToList = new List<CompanyHierarchyItemViewModel>();
        }
    }
    public class UpdateNodesEmployeeCount
    {
        public int Level { get; set; }
        public Guid ItemId { get; set; }
        public int EmployeeCount { get; set; }
        //public List<CompanyHierarchyItemViewModel> Nodes { get; set; }
    }

    public class HierarchyItemViewModelV2
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ParentCompanyHierarchyItemId { get; set; }
        public int EmployeeCount { get; set; }
        public CompnayHierarchyLevel HierarchyLevel { get; set; }
    }

    public class MoveHierarchyItemViewModelV2
    {
        public Guid FromId { get; set; }
        public Guid ToId { get; set; }

    }
}
