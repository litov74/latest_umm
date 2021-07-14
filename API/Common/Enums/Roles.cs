using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Common.Enums
{
    public class Roles
    {
        public const string GlobalAdmin = "GlobalAdmin";
        public const string User = "User";
        public const string All = "GlobalAdmin,User";
    }
}
