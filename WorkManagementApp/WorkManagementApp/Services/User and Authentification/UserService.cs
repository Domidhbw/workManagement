using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WorkManagementApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkManagementApp.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public UserService(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Hole Benutzer nach ID
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        // Hole alle Benutzer
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        // Hole Rollen eines Benutzers
        public async Task<IList<string>> GetUserRolesAsync(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        // Registriere einen neuen Benutzer
        public async Task<User> RegisterUserAsync(User user, string password, string role)
        {
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                var roleExist = await _roleManager.RoleExistsAsync(role);
                if (!roleExist)
                {
                    await _roleManager.CreateAsync(new Role(role));
                }

                await _userManager.AddToRoleAsync(user, role);
                return user;
            }

            return null;
        }

        // Login eines Benutzers mit Username und Passwort
        public async Task<User> LoginAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null && await _userManager.CheckPasswordAsync(user, password))
            {
                return user;
            }
            return null;
        }

        // Prüfe, ob ein Benutzer existiert
        public async Task<bool> UserExistsAsync(string username)
        {
            return await _userManager.FindByNameAsync(username) != null;
        }

        // Benutzer aktualisieren
        public async Task<IdentityResult> UpdateUserAsync(User user, string newRole)
        {
            // Überprüfe, ob eine neue Rolle angegeben ist und ob der Benutzer bereits Rollen hat
            var currentRoles = await _userManager.GetRolesAsync(user);

            // Falls die neue Rolle anders ist als die aktuelle Rolle, aktualisiere die Rolle des Benutzers
            if (newRole != null && !currentRoles.Contains(newRole))
            {
                // Entferne den Benutzer aus allen aktuellen Rollen
                foreach (var role in currentRoles)
                {
                    await _userManager.RemoveFromRoleAsync(user, role);
                }

                // Falls die Rolle existiert, füge den Benutzer zur neuen Rolle hinzu
                var roleExist = await _roleManager.RoleExistsAsync(newRole);
                if (roleExist)
                {
                    await _userManager.AddToRoleAsync(user, newRole);
                }
            }

            // Aktualisiere die Benutzerdaten
            return await _userManager.UpdateAsync(user);
        }


        // Benutzer löschen
        public async Task<IdentityResult> DeleteUserAsync(int id)
        {
            User user = await GetUserByIdAsync(id);
            if (user != null)
            {
                return await _userManager.DeleteAsync(user);
            }
            return IdentityResult.Failed(new IdentityError { Code = "UserNotFound", Description = $"User with ID {id} not found." });
        }
    }
}
