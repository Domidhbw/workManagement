using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WorkManagementApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace WorkManagementApp.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public UserRepository(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id); // Keine Änderung notwendig
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync(); // Keine Änderung notwendig
        }

        public async Task AddAsync(User user)
        {
            await _userManager.CreateAsync(user); // Nutzung von UserManager zum Erstellen eines Benutzers
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user); // Keine Änderung notwendig
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await GetByIdAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Users.AnyAsync(u => u.Id == id); 
        }

        // Geänderte Methode zur Benutzerabfrage anhand des UserName
        public async Task<User> GetUserByUsernameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName); // Benutzername mit UserName ersetzen
        }
    }
}
