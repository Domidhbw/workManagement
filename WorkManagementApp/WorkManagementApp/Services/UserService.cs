using WorkManagementApp.Models;
using WorkManagementApp.Repositories;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace WorkManagementApp.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;  // Repository für Benutzeroperationen
        private readonly UserManager<User> _userManager;    // UserManager für Identity-Funktionen

        public UserService(IRepository<User> userRepository, UserManager<User> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        // Hole Benutzer nach ID
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        // Hole alle Benutzer
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        // Registriere einen neuen Benutzer
        public async Task<User> RegisterUserAsync(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);  // Benutzer mit Passwort erstellen
            if (result.Succeeded)
            {
                return user;  // Wenn erfolgreich, Benutzer zurückgeben
            }

            return null;  // Bei Fehlern null zurückgeben
        }

        // Login eines Benutzers mit Username und Passwort
        public async Task<User> LoginAsync(string username, string password)
        {
            var users = await _userRepository.GetAllAsync();  // Alle Benutzer holen
            var user = users.FirstOrDefault(u => u.UserName == username);  // Nach Benutzername suchen
            if (user != null && await _userManager.CheckPasswordAsync(user, password))  // Passwort checken
            {
                return user;  // Wenn Passwort korrekt, Benutzer zurückgeben
            }
            return null;  // Falls Benutzer nicht gefunden oder Passwort falsch, null zurückgeben
        }

        // Prüfe, ob ein Benutzer mit einem bestimmten Username existiert
        public async Task<bool> UserExistsAsync(string username)
        {
            var users = await _userRepository.GetAllAsync();  // Alle Benutzer holen
            var user = users.FirstOrDefault(u => u.UserName == username);  // Nach Benutzernamen suchen
            return user != null;  // Wenn Benutzer gefunden, true zurückgeben
        }

        // Aktualisiere einen bestehenden Benutzer
        public async Task UpdateUserAsync(User user)
        {
            await _userRepository.UpdateAsync(user);  // Benutzer über Repository aktualisieren
        }

        // Lösche einen Benutzer anhand der ID
        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteAsync(id);  // Benutzer löschen
        }
    }
}
