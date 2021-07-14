using API.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Common.Services
{
    public class UserClaimService : IUserClaimService
    {

        private readonly IHttpContextAccessor _context;
        public UserClaimService(IHttpContextAccessor context)
        {
            _context = context;
        }

        private string GetClaimValueFromType(string claimType)
        {
            var identity = _context.HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var claimData = identity.Claims.FirstOrDefault(x => x.Type.ToLower() == claimType.ToLower());
                if (claimData != null)
                {
                    return claimData.Value;
                }
            }
            return null;
        }

        //private string GetHeaderValue(string headerName)
        //{
        //    return _context.HttpContext.Request.Headers[headerName].FirstOrDefault();
        //}

        public string GetCurrentUserId()
        {
            return GetClaimValueFromType("Id");
           
        }

        public string GetCurrentUserEmail()
        {
            return GetClaimValueFromType("Email");
        }
        public string GetCurrentUserCompanyId()
        {
            return GetClaimValueFromType("CompanyId");
        }
        public string GetCurrentUserRole()
        {
            return GetClaimValueFromType("RoleName");
        }
    }

}
