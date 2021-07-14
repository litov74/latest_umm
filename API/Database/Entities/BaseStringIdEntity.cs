using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Services;

namespace API.Models.Entities
{
    public abstract class BaseStringIdEntity : BaseTrackingEntity
    {
        [MaxLength(36)]
        public string Id { get; set; }
        public BaseStringIdEntity()
        {
            Id = FuncHelppers.GenerateGUID();
        }
    }
}
