using Microsoft.AspNetCore.Mvc;
using WorkManagementApp.DTOs;
using WorkManagementApp.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using WorkManagementApp.Services.Authentification;

namespace WorkManagementApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService; // Service zum Verwalten der Benutzer
        private readonly IAuthService _authService; // Service zum Erstellen des JWT-Tokens

        public AuthController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (await _userService.UserExistsAsync(model.Username))
            {
                return BadRequest("Benutzername ist bereits vergeben.");
            }

            var user = new User
            {
                UserName = model.Username,
                Email = model.Email
            };

            var result = await _userService.RegisterUserAsync(user, model.Password, model.Role);
            if (result == null)
            {
                return BadRequest("Benutzer konnte nicht erstellt werden.");
            }

            return Ok(new { message = "Benutzer erfolgreich erstellt." });
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            // Versucht, den Benutzer basierend auf dem LoginDto zu finden und zu authentifizieren.
            var user = await _userService.LoginAsync(model.Username, model.Password);

            if (user == null)
            {
                return Unauthorized("Ungültige Anmeldedaten.");
            }

            // Wenn der Benutzer erfolgreich authentifiziert wurde, wird ein JWT-Token erstellt
            var token = await _authService.GenerateJwtToken(user);

            // Rückgabe des Tokens als Antwort auf die erfolgreiche Anmeldung
            return Ok(new { token });
        }
    }
}
