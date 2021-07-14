using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Common.Interfaces
{
   public interface IUserClaimService
    {
        string GetCurrentUserId();
        string GetCurrentUserEmail();
        string GetCurrentUserCompanyId();
        string GetCurrentUserRole();
    }
}
