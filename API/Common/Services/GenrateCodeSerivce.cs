using System;
using System.Linq;
using API.Common.Enums;
using API.Configuration;
using Microsoft.Extensions.Configuration;

namespace API.Common.Services
{
    public class GenrateCodeSerivce
    {

        private readonly AppSettings.CoreLogicSettingModel _coreLogicSetting;
        private readonly CryptographyService _cryptographyService;
        public GenrateCodeSerivce(IConfiguration configuration, CryptographyService cryptographyService)
        {
            var appSettings = configuration.Get<AppSettings>();
            _coreLogicSetting = appSettings.CoreLogicSettings;
            _cryptographyService = cryptographyService;
        }

        /// <summary>
        /// Fotmat code will be: DateExpired-RandomCode
        /// </summary>
        /// <returns></returns>
        public string GenarateVerifyCode(VerifyTypeEnum verifyType)
        {
            var plantextCode = $"{verifyType}_{RandomService.RandomString(32)}_{DateTime.UtcNow.AddSeconds(_coreLogicSetting.EmailVerifyExpriedInSeconds)}";
            var encryptcode = _cryptographyService.Encryption(plantextCode);
            return encryptcode;
        }
        /// <summary>
        /// Verify Code Is valid
        /// </summary>
        /// <param name="encryptCode"></param>
        /// <returns></returns>
        public bool VerifyCodeDateAndType(string encryptCode, VerifyTypeEnum verifyType)
        {
            var plantextCode = _cryptographyService.Decryption(encryptCode);
            var splitText = plantextCode.Split("_");
            DateTime.TryParse(splitText.Last(), out DateTime expriedDate);
            Enum.TryParse(splitText.First(), out VerifyTypeEnum type);

            return expriedDate > DateTime.UtcNow && verifyType == type;
        }

    }
}
