using WorkManagementApp.Models;
using Task = System.Threading.Tasks.Task;

public interface IUserService
{
    Task<User> GetUserByIdAsync(int id);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> RegisterUserAsync(User user, string password);
    Task<User> LoginAsync(string username, string password);
    Task<bool> UserExistsAsync(string username);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(int id);
}