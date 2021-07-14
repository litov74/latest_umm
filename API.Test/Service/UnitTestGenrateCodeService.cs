using API.Common.Enums;
using API.Common.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace API.Test.Service
{
    [TestClass]
    public class UnitTestGenrateCodeService
    {
        [TestMethod]
        public void TestGenarateCodeAndVerifyTimeValidFailed()
        {
            var cryptoService = new CryptographyService(TestSettings.configuration);
            var authenticateService = new GenrateCodeSerivce(TestSettings.configuration, cryptoService);
            var code =  authenticateService.GenarateVerifyCode(VerifyTypeEnum.FogotPassword);
            var isVaid =  authenticateService.VerifyCodeDateAndType(code, VerifyTypeEnum.VerifyEmail);
            Assert.IsFalse(isVaid);
        }

        [TestMethod]
        public void TestGenarateCodeAndVerifyTimeValidSuccessed()
        {
            var cryptoService = new CryptographyService(TestSettings.configuration);
            var authenticateService = new GenrateCodeSerivce(TestSettings.configuration, cryptoService);
            var code = authenticateService.GenarateVerifyCode(VerifyTypeEnum.FogotPassword);
            var isVaid = authenticateService.VerifyCodeDateAndType(code, VerifyTypeEnum.FogotPassword);
            Assert.IsTrue(isVaid);
        }
    }
}
