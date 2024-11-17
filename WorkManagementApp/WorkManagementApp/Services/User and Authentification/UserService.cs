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
        private readonly UserManager<User> _userManager;  // Manages user-related operations (e.g., registration, login, etc.)
        private readonly RoleManager<Role> _roleManager;  // Manages roles for users (e.g., creating roles, assigning roles)

        // Constructor to inject UserManager and RoleManager dependencies
        public UserService(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Method to get a user by their ID
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);  // Finds user by ID
        }

        // Method to get all users in the system
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();  // Returns a list of all users
        }

        // Method to get the roles of a specific user
        public async Task<IList<string>> GetUserRolesAsync(User user)
        {
            return await _userManager.GetRolesAsync(user);  // Returns the roles of the user
        }

        // Method to register a new user with roles
        public async Task<User> RegisterUserAsync(User user, string password, string rolesString)
        {
            // Create the user with the provided password
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // Split the roles from the comma-separated string and trim any whitespace
                var roles = rolesString.Split(',')
                                       .Select(role => role.Trim())
                                       .Where(role => !string.IsNullOrEmpty(role))  // Ignore empty roles
                                       .ToList();

                // Check if each role exists, and create the role if it doesn't
                foreach (var role in roles)
                {
                    var roleExist = await _roleManager.RoleExistsAsync(role);
                    if (!roleExist)
                    {
                        await _roleManager.CreateAsync(new Role(role));  // Create the role if it doesn't exist
                    }
                    await _userManager.AddToRoleAsync(user, role);  // Assign the role to the user
                }

                return user;  // Return the created user
            }

            // If user creation fails, return null
            return null;
        }

        // Method to login a user using their username and password
        public async Task<User> LoginAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);  // Find the user by username
            if (user != null && await _userManager.CheckPasswordAsync(user, password))  // Check password
            {
                return user;  // Return the user if credentials are valid
            }
            return null;  // Return null if login failed
        }

        // Method to check if a user exists by their username
        public async Task<bool> UserExistsAsync(string username)
        {
            return await _userManager.FindByNameAsync(username) != null;  // Check if the user exists
        }

        // Method to update a user's roles
        public async Task<IdentityResult> UpdateUserAsync(User user, string rolesString)
        {
            // Split and clean up the role list from the string
            var roles = rolesString.Split(',')
                                   .Select(role => role.Trim())
                                   .Where(role => !string.IsNullOrEmpty(role))  // Ignore empty roles
                                   .ToList();

            // Get the current roles of the user
            var currentRoles = await _userManager.GetRolesAsync(user);

            // Remove the user from their current roles
            foreach (var role in currentRoles)
            {
                await _userManager.RemoveFromRoleAsync(user, role);
            }

            // Add the user to the new roles
            foreach (var role in roles)
            {
                var roleExist = await _roleManager.RoleExistsAsync(role);
                if (roleExist)
                {
                    await _userManager.AddToRoleAsync(user, role);  // Add the user to the existing role
                }
                else
                {
                    // Optional: Create the role if it doesn't exist and assign it
                    await _roleManager.CreateAsync(new Role(role));
                    await _userManager.AddToRoleAsync(user, role);
                }
            }

            // Update the user in the database
            return await _userManager.UpdateAsync(user);  // Return the result of the update operation
        }

        // Method to delete a user by their ID
        public async Task<IdentityResult> DeleteUserAsync(int id)
        {
            User user = await GetUserByIdAsync(id);  // Find the user by ID
            if (user != null)
            {
                return await _userManager.DeleteAsync(user);  // Delete the user if found
            }
            return IdentityResult.Failed(new IdentityError { Code = "UserNotFound", Description = $"User with ID {id} not found." });  // Return failure if user not found
        }
    }
}
