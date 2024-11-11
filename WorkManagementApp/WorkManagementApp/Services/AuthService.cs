using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WorkManagementApp.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

namespace WorkManagementApp.Services
{
    public class AuthService : IAuthService
    {
        private readonly string _jwtSecretKey;

        // Der Konstruktor empfängt IConfiguration, um auf die Konfigurationswerte zuzugreifen
        public AuthService(IConfiguration configuration)
        {
            // Den SecretKey aus der appsettings.json lesen
            _jwtSecretKey = configuration["JwtSettings:SecretKey"];
        }

        // Diese Methode erstellt das JWT für den Benutzer
        public async Task<string> GenerateJwtToken(User user)
        {
            // Der geheime Schlüssel für das Token wird hier definiert.
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Das Token wird mit Claims (z.B. Benutzername, Rollen) und einer Ablaufzeit erstellt.
            var tokenDescriptor = new JwtSecurityToken(
                issuer: "https://deinewebsite.com", // Hier kannst du die Aussteller-URL setzen
                audience: "https://deinewebsite.com", // Die Zielgruppe des Tokens
                claims: null, // Hier können benutzerdefinierte Claims (wie Rollen) hinzugefügt werden
                expires: DateTime.Now.AddHours(1), // Das Token läuft nach einer Stunde ab
                signingCredentials: credentials // Die Signierung des Tokens
            );

            // Das Token wird aus dem deskriptiven Token-Objekt erzeugt
            var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

            return token; // Das generierte JWT wird zurückgegeben
        }
    }
}
