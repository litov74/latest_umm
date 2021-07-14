using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace API.Utility
{
    public static class StripeConfigurationParam
    {
        public static string UserCountParamName => ConfigurationManager.AppSettings["userCount"];
        public static string NewPlanMarker => ConfigurationManager.AppSettings["newPlanMarker"];
        public static string ApiSign => ConfigurationManager.AppSettings["StripeApiSign"];
        public static int TrialInterval => Convert.ToInt32(ConfigurationManager.AppSettings["TrialInterval"]);
        public static int TrialUserCount => Convert.ToInt32(ConfigurationManager.AppSettings["TrialUserCount"]);
    }
}
