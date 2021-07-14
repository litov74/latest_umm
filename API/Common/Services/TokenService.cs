using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using API.Configuration;
using API.Data;
using API.Models.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Common.Services
{
    public class TokenService
    {
        private readonly AppSettings.JwtSettingsModel _jwtSettings;
        private readonly APIContext _context;

        public TokenService(IConfiguration configuration, APIContext context)
        {
            var appSettings = configuration.Get<AppSettings>();
            _jwtSettings = appSettings.JwtSettings;
            _context = context;
        }

        public (string accessToken, string tokenType, int expiresIn) GenerateAccessToken(UserEntity account, Dictionary<string, string> additionalClaims = null, bool isRememberMe = false)
        {
            var expriredSeconds = isRememberMe ? _jwtSettings.ExpiredForRememberMeInSeconds : _jwtSettings.ExpiredInSeconds;
            var accessToken = GenerateJSONWebToken(account, expriredSeconds, additionalClaims);
            return (accessToken, "Bearer", expriredSeconds);
        }

        private string GenerateJSONWebToken(UserEntity account, int expiredInSeconds, Dictionary<string, string> additionalClaims = null)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var getCompanyId = (from user in _context.UserEntity
                                join usercompany in _context.CompanyUserMappingEntity on user.Id equals usercompany.UserId
                                where account.Id == usercompany.UserId
                                select new
                                {
                                    usercompany.CompanyId
                                }).FirstOrDefault();
            var claims = new List<Claim>
            {
                //new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                 new Claim("Id", account.Id.ToString()),
                new Claim("Email", account.Email),
                new Claim("CompanyId",getCompanyId.CompanyId.ToString()),
                new Claim(ClaimTypes.Name,account.FullName.ToString()),
                //new Claim(ClaimTypes.Email, account.Email),
            };

            if (additionalClaims?.Any() == true)
            {
                foreach (var item in additionalClaims)
                {
                    claims.Add(new Claim("RoleName", item.Value/*,item.Key*/));
                }
            }

            var token = new JwtSecurityToken(null,
                null,
                claims,
                expires: DateTime.UtcNow.AddSeconds(expiredInSeconds),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
