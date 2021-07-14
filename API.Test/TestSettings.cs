using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace API.Test
{
    public static class TestSettings
    {
        public static IConfiguration configuration = new ConfigurationBuilder()
        .AddInMemoryCollection(new Dictionary<string, string> {
            {"CryptographySettings:AesPrivateKeyBase64", "dGhpc19pc19hX3NlY3JldA=="},
            {"JwtSettings:ExpiredInSeconds", "6400"},
            {"JwtSettings:ExpiredForRememberMeInSeconds", "2592000"},
            {"JwtSettings:Algorithm", "HmacSha256Signature"},
            {"JwtSettings:Issuer", "thechangecompass.com"},
            {"CoreLogicSettings:EmailVerifyExpriedInSeconds", "86400"},
        })
        .Build();

    }
}
