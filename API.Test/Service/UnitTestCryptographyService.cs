using API.Common.Services;
using API.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace API.Test.Service
{
    [TestClass]
    public class UnitTestCryptographyService
    {

        [TestMethod]
        public void TestEncryptAndDecrypt()
        {
            CryptographyService cryptographyService = new CryptographyService(TestSettings.configuration);
            var plantext = "This is a text";
            var encryptText = cryptographyService.Encryption(plantext);
            var decodeText = cryptographyService.Decryption(encryptText);
            Assert.AreEqual(plantext, decodeText);

        }
    }
}
