using API.Common.Interfaces;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Common.Services
{
    public class LogService:ILogService
    {
        private readonly Logger _logger;
        public LogService()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        public void LogError(Exception ex, string errorMessage)
        {
            _logger.Error(ex, errorMessage);
        }

        public void LogInfo(string message)
        {
            _logger.Info(message);
        }
    }
}
