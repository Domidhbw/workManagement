using Microsoft.AspNetCore.Mvc;
using WorkManagementApp.Models;
using WorkManagementApp.DTOs;
using WorkManagementApp.Services;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;

namespace WorkManagementApp.Controllers
{
    // API Controller für Benutzermanagement
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        // Konstruktor der UserController-Klasse, der das IUserService injiziert
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/users - Holt alle Benutzer
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllUsersAsync(); // Ruft alle Benutzer ab

            if (users == null || !users.Any())
            {
                return NoContent(); // Gibt 204 zurück, wenn keine Benutzer gefunden wurden
            }

            // Initialisiert eine Liste von UserDto, um die Benutzerinformationen zurückzugeben
            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await _userService.GetUserRolesAsync(user); // Holt die Rollen des Benutzers

                var userDto = new UserDto
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                    Roles = roles.ToList() // Fügt die Rollen zur UserDto-Liste hinzu
                };

                userDtos.Add(userDto);
            }

            return Ok(userDtos); // Gibt die Liste der Benutzer als Antwort zurück
        }

        // GET: api/users/{id} - Holt einen einzelnen Benutzer basierend auf der Benutzer-ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id); // Holt den Benutzer anhand der ID

            if (user == null)
            {
                return NotFound(); // Gibt 404 zurück, wenn der Benutzer nicht gefunden wurde
            }

            var roles = await _userService.GetUserRolesAsync(user); // Holt die Rollen des Benutzers

            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Roles = roles.ToList() // Fügt die Rollen zur UserDto-Liste hinzu
            };

            return Ok(userDto); // Gibt den Benutzer mit den Rollen zurück
        }

        // PUT: api/users/{id} - Aktualisiert die Daten eines bestehenden Benutzers
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto updatedUserDto)
        {
            // Überprüft, ob die Eingabedaten korrekt sind
            if (id == 0 || updatedUserDto == null)
            {
                return BadRequest("Ungültige Eingabedaten."); // Gibt eine BadRequest-Antwort zurück, wenn die Eingaben ungültig sind
            }

            var user = await _userService.GetUserByIdAsync(id); // Ruft den Benutzer anhand der ID ab
            if (user == null)
            {
                return NotFound(); // Gibt 404 zurück, wenn der Benutzer nicht gefunden wurde
            }

            // Setzt die Benutzerinformationen aus dem UpdateUserDto
            user.Email = updatedUserDto.Email ?? user.Email; // Aktualisiert die E-Mail, wenn sie im DTO enthalten ist
            user.UserName = updatedUserDto.UserName ?? user.UserName; // Aktualisiert den Benutzernamen, wenn er im DTO enthalten ist

            // Ruft die Methode zum Aktualisieren des Benutzers auf
            await _userService.UpdateUserAsync(user, updatedUserDto.Role);

            return NoContent(); // Gibt 204 No Content zurück, da keine Daten im Body der Antwort benötigt werden
        }

        // DELETE: api/users/{id} - Löscht einen Benutzer anhand der ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetUserByIdAsync(id); // Ruft den Benutzer anhand der ID ab
            if (user == null)
            {
                return NotFound(); // Gibt 404 zurück, wenn der Benutzer nicht gefunden wurde
            }

            // Ruft die Methode zum Löschen des Benutzers auf
            await _userService.DeleteUserAsync(id);
            return NoContent(); // Gibt 204 No Content zurück, da keine Antwortdaten erforderlich sind
        }
    }
}
