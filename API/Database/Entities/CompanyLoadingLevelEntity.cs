using API.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Database.Entities
{
    public class CompanyLoadingLevelEntity:BaseStringIdEntity
    {
        public int Week { get; set; }
        public int Level { get; set; }
        public string CompanyId { get; set; }
        public string HierarchyItemId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public virtual CompanyEntity Company { get; set; }
        public virtual CompanyHierarchyItem HierarchyItem { get; set; }
    }
}
