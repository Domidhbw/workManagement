using WorkManagementApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

public interface IUserService
{
    Task<User> GetUserByIdAsync(int id);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> RegisterUserAsync(User user, string password, string role);
    Task<User> LoginAsync(string username, string password);
    Task<bool> UserExistsAsync(string username);
    Task<IdentityResult> UpdateUserAsync(User user);  // Rückgabe von IdentityResult
    Task<IdentityResult> DeleteUserAsync(int id);
    Task<IList<string>> GetUserRolesAsync(User user);
}
