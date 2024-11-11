using Microsoft.IdentityModel.Tokens;
using WorkManagementApp.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

namespace WorkManagementApp.Services
{
    public class TokenService : ITokenService
    {
        private readonly string _jwtSecretKey = "dein_geheimer_schlüssel";  // Verwende einen sicheren Schlüssel

        // JWT-Token generieren
        public async Task<string> GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: "https://deinewebsite.com",
                audience: "https://deinewebsite.com",
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            return token;
        }
    }
}
