using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Common.Interfaces
{
    public interface ILogService
    {
        void LogError(Exception ex, string errorMessage);
        void LogInfo(string message);
    }
}
