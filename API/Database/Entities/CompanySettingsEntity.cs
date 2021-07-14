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
    public class CompanySettingsEntity:BaseStringIdEntity
    {
        [Key, Column(Order = 1)]
        public CompanySettingsEnum Key { get; set; }
        public string Value { get; set; }
        public string CompanyId { get; set; }
        public bool WithImpactlvl0 { get; set; }
        [ForeignKey("CompanyId")]
        public CompanyEntity Company { get; set; }
    }
}
