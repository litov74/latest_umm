using API.Common.Services;
using API.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace API.Test.Service
{
    [TestClass]
    public class UnitTestHashingService
    {

        [TestMethod]
        public void TestHashAndVerifyPassword()
        {
            var hashingService = new HashingService();
            var password = "thisisapasswod";
            var passwordHash = hashingService.HashPassword(password);
            var isEqual =  hashingService.VerifyHashedPassword(passwordHash, password);
            Assert.IsTrue(isEqual);
        }
    }
}
