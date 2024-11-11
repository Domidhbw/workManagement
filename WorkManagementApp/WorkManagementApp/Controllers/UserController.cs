using Microsoft.AspNetCore.Mvc;
using WorkManagementApp.Models;
using WorkManagementApp.Services;
using System.Threading.Tasks;

namespace WorkManagementApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        // Constructor
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/users
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Wir holen alle Benutzer über den Service
            var users = await _userService.GetAllUsersAsync();
            if (users == null || !users.Any())
            {
                return NoContent(); // Gibt 204 No Content zurück, wenn keine Benutzer vorhanden sind.
            }
            return Ok(users); // Gibt die Liste der Benutzer zurück.
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            // Benutzer anhand der ID über den Service finden
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(); // Gibt 404 zurück, wenn der Benutzer nicht gefunden wurde.
            }
            return Ok(user); // Gibt den Benutzer zurück.
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] User updatedUser)
        {
            // Sicherstellen, dass die IDs übereinstimmen
            if (id != updatedUser.Id)
            {
                return BadRequest(); // Gibt 400 Bad Request zurück, wenn die IDs nicht übereinstimmen.
            }

            // Benutzer anhand der ID suchen
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(); // Gibt 404 zurück, wenn der Benutzer nicht gefunden wurde.
            }

            // Benutzer über den Service aktualisieren
            await _userService.UpdateUserAsync(updatedUser);
            return NoContent(); // Gibt 204 No Content zurück, wenn die Aktualisierung erfolgreich war.
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Benutzer anhand der ID suchen
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(); // Gibt 404 zurück, wenn der Benutzer nicht gefunden wurde.
            }

            // Benutzer über den Service löschen
            await _userService.DeleteUserAsync(id);
            return NoContent(); // Gibt 204 No Content zurück, wenn der Benutzer erfolgreich gelöscht wurde.
        }
    }
}
