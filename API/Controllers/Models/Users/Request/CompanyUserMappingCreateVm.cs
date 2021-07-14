using API.Common.Enums;

namespace API.Controllers.Models.Users.Request
{
    public class CompanyUserMappingCreateVm
    {
        public string Id { get; set; }
        public CompanyAccessLevelEnum AccessLevel { get; set; }
        public bool IsActive { get; set; }
    }
}
