using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WorkManagementApp.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace WorkManagementApp.Services.Authentification
{
    public class AuthService : IAuthService
    {
        private readonly string _jwtSecretKey;  // Secret key for signing the JWT
        private readonly UserManager<User> _userManager;  // Manages user-related operations (e.g., roles)

        // Constructor to inject IConfiguration and UserManager services
        public AuthService(IConfiguration configuration, UserManager<User> userManager)
        {
            // Reading the secret key from configuration (appsettings.json)
            _jwtSecretKey = configuration["JwtSettings:SecretKey"];
            _userManager = userManager;
        }

        // Method to generate a JWT token for a user
        public async Task<string> GenerateJwtToken(User user)
        {
            // Creating the security key using the secret key from configuration
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecretKey));

            // Signing credentials that will be used to sign the JWT
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Get the roles assigned to the user
            var roles = await _userManager.GetRolesAsync(user);

            // Create a list of claims to be added to the JWT
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),  // Add username as a claim
                new Claim(ClaimTypes.Email, user.Email)    // Add email as a claim
            };

            // Add roles as claims to the JWT
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));  // Add each role as a claim
            }

            // Create the token descriptor, which defines the structure of the JWT
            var tokenDescriptor = new JwtSecurityToken(
                issuer: "https://deinewebsite.com",    // Issuer of the token (could be your API)
                audience: "https://deinewebsite.com",  // Audience for which the token is intended
                claims: claims,                       // Claims to be included in the token (user data and roles)
                expires: DateTime.Now.AddHours(1),    // Set the expiration time of the token (1 hour)
                signingCredentials: credentials       // Sign the token using the signing credentials
            );

            // Generate the JWT token as a string from the token descriptor
            var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

            return token;  // Return the generated JWT token
        }
    }
}
