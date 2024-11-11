using WorkManagementApp.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WorkManagementApp.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        // Hole Benutzer nach ID
        public async Task<User> GetUserByIdAsync(int id)
        {
            // Hier wird der Benutzer anhand der ID gefunden (UserManager verwendet string als ID, prüfe ID-Umwandlung falls nötig)
            return await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        // Hole alle Benutzer
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        // Registriere einen neuen Benutzer
        public async Task<User> RegisterUserAsync(User user, string password, string role = "Mitarbeiter")
        {
            // Benutzer mit Passwort erstellen
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // Wenn die Rolle noch nicht existiert, erstelle sie
                var roleExist = await _roleManager.RoleExistsAsync(role);
                if (!roleExist)
                {
                    // Rolle erstellen, wenn sie nicht existiert
                    await _roleManager.CreateAsync(new Role(role));
                }

                // Weisen Sie den Benutzer der Rolle zu
                await _userManager.AddToRoleAsync(user, role);

                return user;  // Wenn erfolgreich, Benutzer zurückgeben
            }

            return null;  // Bei Fehlern null zurückgeben
        }


        // Login eines Benutzers mit Username und Passwort
        public async Task<User> LoginAsync(string username, string password)
        {
            // Benutzer anhand des Benutzernamens abrufen
            var user = await _userManager.FindByNameAsync(username);
            if (user != null && await _userManager.CheckPasswordAsync(user, password))  // Passwort checken
            {
                return user;  // Wenn Passwort korrekt, Benutzer zurückgeben
            }
            return null;  // Falls Benutzer nicht gefunden oder Passwort falsch, null zurückgeben
        }

        // Prüfe, ob ein Benutzer mit einem bestimmten Username existiert
        public async Task<bool> UserExistsAsync(string username)
        {
            // Prüfen, ob ein Benutzer mit dem angegebenen Benutzernamen existiert
            return await _userManager.FindByNameAsync(username) != null;
        }

        // Aktualisiere einen bestehenden Benutzer
        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            // Benutzer über UserManager aktualisieren und Ergebnis zurückgeben
            return await _userManager.UpdateAsync(user);
        }

        // Lösche einen Benutzer anhand der ID
        public async Task<IdentityResult> DeleteUserAsync(int id)
        {
            User user = await GetUserByIdAsync(id);
            if (user != null)
            {
                // Benutzer löschen und Ergebnis zurückgeben
                return await _userManager.DeleteAsync(user);
            }
            // Falls Benutzer nicht gefunden wurde, eine Fehlermeldung erstellen
            return IdentityResult.Failed(new IdentityError
            {
                Code = "UserNotFound",
                Description = $"User with ID {id} not found."
            });
        }

    }
}
