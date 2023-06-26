using FindJobs.Domain.Dtos;
using FindJobs.Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FindJobs.Infrastructure.Services
{
    public class JWTManagerService : IJWTManagerService
    {
        private readonly IConfiguration configuration;

        public JWTManagerService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }



        public string GenerateToken(TokenParametersDto tokenParameters)
        {
            Claim applicantRole = null;
            Claim companyRole = null;
            if (tokenParameters.IsApplicant)
            {
                applicantRole = new Claim(ClaimTypes.Role, nameof(tokenParameters.IsApplicant));
            }
            if (tokenParameters.IsCompany)
            {
                companyRole = new Claim(ClaimTypes.Role, nameof(tokenParameters.IsCompany));
            }
            // Else we generate JSON Web Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(configuration["AuthSettings:JwtKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
              {
             new Claim(ClaimTypes.NameIdentifier, tokenParameters.Email),
             applicantRole,
             companyRole
              }),
                Expires = DateTime.UtcNow.AddMonths(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


    }
}
