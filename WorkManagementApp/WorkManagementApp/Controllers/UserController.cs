using Microsoft.AspNetCore.Mvc;
using WorkManagementApp.Models;
using WorkManagementApp.DTOs;
using WorkManagementApp.Services;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;

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
            var users = await _userService.GetAllUsersAsync();

            if (users == null || !users.Any())
            {
                return NoContent(); // Gibt 204 No Content zurück, wenn keine Benutzer vorhanden sind.
            }

            // Mapping von User auf UserDto
            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await _userService.GetUserRolesAsync(user); // Rollen des Benutzers abrufen

                var userDto = new UserDto
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                    Roles = roles.ToList() // Alle Rollen als Liste
                };

                userDtos.Add(userDto);
            }

            return Ok(userDtos); // Gibt die Liste der Benutzer zurück.
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound(); // Gibt 404 zurück, wenn der Benutzer nicht gefunden wurde.
            }

            var roles = await _userService.GetUserRolesAsync(user); // Rollen des Benutzers abrufen

            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Roles = roles.ToList() // Alle Rollen als Liste
            };

            return Ok(userDto); // Gibt den Benutzer zurück.
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto updatedUserDto)
        {
            if (id == 0 || updatedUserDto == null)
            {
                return BadRequest("Ungültige Eingabedaten.");
            }

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Email = updatedUserDto.Email ?? user.Email;
            user.UserName = updatedUserDto.UserName ?? user.UserName;

            await _userService.UpdateUserAsync(user, updatedUserDto.Role);

            return NoContent();
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
    }
}
