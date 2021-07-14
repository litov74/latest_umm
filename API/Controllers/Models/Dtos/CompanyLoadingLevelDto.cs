using API.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.Models.Dtos
{
    public class CompanyLoadingLevelDto:BaseDto
    {
        public int Week { get; set; }
        public int Level { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public virtual CompanyDto Company { get; set; }
        public virtual CompanyHierarchyItemsDto HierarchyItem { get; set; }
    }
}
