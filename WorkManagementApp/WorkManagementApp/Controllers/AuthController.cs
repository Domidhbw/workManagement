using Microsoft.AspNetCore.Mvc;
using WorkManagementApp.DTOs;
using WorkManagementApp.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using WorkManagementApp.Services.Authentification;

namespace WorkManagementApp.Controllers
{
    // Route-Attribut legt die Basis-URL für diesen Controller fest
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // Service zum Verwalten der Benutzer, wie Registrierung und Authentifizierung
        private readonly IUserService _userService;
        // Service zum Erstellen und Verwalten des JWT-Tokens für die Authentifizierung
        private readonly IAuthService _authService;

        // Konstruktor zur Initialisierung der Services, die der Controller benötigt
        public AuthController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        // POST: api/auth/register
        // Diese Methode wird zum Registrieren eines neuen Benutzers verwendet
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            // Überprüfen, ob der Benutzername bereits existiert
            if (await _userService.UserExistsAsync(model.Username))
            {
                // Rückgabe einer Fehlermeldung, wenn der Benutzername schon vergeben ist
                return BadRequest("Benutzername ist bereits vergeben.");
            }

            // Erstellen eines neuen Benutzerobjekts mit den Daten aus dem RegisterDto
            var user = new User
            {
                UserName = model.Username,
                Email = model.Email,
            };

            // Registrierung des neuen Benutzers und Zuweisung der Rolle
            var result = await _userService.RegisterUserAsync(user, model.Password, model.Role);

            // Überprüfen, ob die Registrierung erfolgreich war
            if (result == null)
            {
                return BadRequest("Benutzer konnte nicht erstellt werden.");
            }

            // Rückgabe einer Erfolgsmeldung, wenn der Benutzer erfolgreich erstellt wurde
            return Ok(new { message = "Benutzer erfolgreich erstellt." });
        }

        // POST: api/auth/login
        // Diese Methode wird für das Login eines Benutzers verwendet
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            // Versuchen, den Benutzer mit den übergebenen Anmeldedaten zu finden und zu authentifizieren
            var user = await _userService.LoginAsync(model.Username, model.Password);

            // Wenn der Benutzer nicht gefunden oder die Anmeldedaten ungültig sind
            if (user == null)
            {
                return Unauthorized("Ungültige Anmeldedaten.");
            }

            // Generieren eines JWT-Tokens für den authentifizierten Benutzer
            var token = await _authService.GenerateJwtToken(user);

            // Rückgabe des JWT-Tokens und der Benutzer-ID als Antwort auf die erfolgreiche Anmeldung
            return Ok(new { token, userId = user.Id });
        }
    }
}
