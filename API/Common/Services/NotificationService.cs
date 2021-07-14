using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using API.Common.Enums;
using API.Configuration;
using Microsoft.Extensions.Configuration;

namespace API.Common.Services
{
    public class NotificationService
    {
        private readonly AppSettings.CoreLogicSettingModel _coreLogicSetting;
        private readonly SendGridEmailService _sendGridEmailService;
        public NotificationService(IConfiguration configuration,
            SendGridEmailService sendGridEmailService)
        {
            var appSettings = configuration.Get<AppSettings>();
            _coreLogicSetting = appSettings.CoreLogicSettings;
            _sendGridEmailService = sendGridEmailService;

        }

        public async Task SendVerifyEmail(string email, string code, string plan)
        {
            var callback = CreateVerifyEmailLink(email, code, plan);
            var payload = new PayloadVerifyEmail
            {
                callbackUrl = callback
            };
            await _sendGridEmailService.SendMailTemplateAsync(_coreLogicSetting.SendgirdTemplateVerifyEmailId, email, payload);
        }
        public async Task SendVerifyEmail(string email, string code)
        {
            var callback = CreateVerifyEmailLink(email, code);
            var payload = new PayloadVerifyEmail
            {
                callbackUrl = callback
            };
            await _sendGridEmailService.SendMailTemplateAsync(_coreLogicSetting.SendgirdTemplateVerifyEmailId, email, payload);
        }


        public async Task SendForgotPassword(string email,string fullName , string code, string browser, string operatingSystem)
        {
            var callback = CreateVerifyForgotPasswordLink(email, code);
            
            var payload = new PayloadForgotPassword
            {
                callbackUrl = callback,
                name = fullName,
                browser_name = browser,
                operating_system = operatingSystem

            };
            await _sendGridEmailService.SendMailTemplateAsync(_coreLogicSetting.SendgirdTemplateRecoveryPasswordId, email, payload);
        }

        private string CreateVerifyForgotPasswordLink(string email, string code)
        {
            return $"{_coreLogicSetting.FrontendUrl}/{_coreLogicSetting.FrontendPathVerifyForgotPassword}?email={email}&code={WebUtility.UrlEncode(code)}";
        }

        private string CreateVerifyEmailLink(string email, string code, string plan = null)
        {
            if (plan != null)
            {
                return $"{_coreLogicSetting.FrontendUrl}/{_coreLogicSetting.FrontendPathVerifyEmail}?email={email}&code={WebUtility.UrlEncode(code)}&plan={plan}";
            }
            return $"{_coreLogicSetting.FrontendUrl}/{_coreLogicSetting.FrontendPathVerifyEmail}?email={email}&code={WebUtility.UrlEncode(code)}";
        }
        private class PayloadVerifyEmail
        {
#pragma warning disable IDE1006 // Naming Styles
            public string callbackUrl { get; set; }
#pragma warning restore IDE1006 // Naming Styles
        }
        private class PayloadForgotPassword
        {
#pragma warning disable IDE1006 // Naming Styles
            public string name { get; set; }
            public string callbackUrl { get; set; }
            public string browser_name { get; set; }
            public string operating_system { get; set; }
#pragma warning restore IDE1006 // Naming Styles
        }
    }
}
