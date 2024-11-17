using Microsoft.EntityFrameworkCore;
using TaskModel = WorkManagementApp.Models.Task;
using System.Threading.Tasks;
using System.Linq;

namespace WorkManagementApp.Repositories.Tasks
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        // Constructor: Initializes the repository with ApplicationDbContext
        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Retrieves all tasks, including their assigned user and project details
        public async Task<IEnumerable<TaskModel>> GetAllAsync()
        {
            // Include 'AssignedTo' (the user the task is assigned to) and 'Project' (the project the task belongs to)
            return await _context.Tasks
                .Include(t => t.AssignedTo)  // Loads the user assigned to the task
                .Include(t => t.Project)     // Loads the project to which the task belongs
                .ToListAsync();  // Asynchronously fetches all tasks
        }

        // Retrieves a task by its ID, including its assigned user and project details
        public async Task<TaskModel> GetByIdAsync(int id)
        {
            // Include 'AssignedTo' (the user) and 'Project' (the project)
            return await _context.Tasks
                .Include(t => t.AssignedTo)
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == id); // Returns the task with the specified ID or null if not found
        }

        // Retrieves all tasks assigned to a specific user by their user ID
        public async Task<IEnumerable<TaskModel>> GetTasksByUserIdAsync(int userId)
        {
            // Filters tasks assigned to a specific user (based on 'AssignedToUserId')
            return await _context.Tasks
                .Include(t => t.AssignedTo)  // Includes the user to whom the task is assigned
                .Include(t => t.Project)     // Includes the project to which the task belongs
                .Where(t => t.AssignedToUserId == userId) // Filters tasks assigned to the specified user ID
                .ToListAsync();  // Asynchronously fetches the filtered tasks
        }

        // Retrieves all tasks belonging to a specific project by its project ID
        public async Task<IEnumerable<TaskModel>> GetTasksByProjectIdAsync(int projectId)
        {
            // Filters tasks that belong to a specific project (based on 'ProjectId')
            return await _context.Tasks
                .Include(t => t.AssignedTo)  // Includes the user assigned to each task
                .Include(t => t.Project)     // Includes the project for each task
                .Where(t => t.ProjectId == projectId)  // Filters tasks belonging to the specified project
                .ToListAsync();  // Asynchronously fetches the filtered tasks
        }

        // Adds a new task to the database
        public async Task AddAsync(TaskModel task)
        {
            // Adds the new task to the Tasks DbSet
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();  // Saves changes to the database
        }

        // Updates an existing task in the database
        public async Task UpdateAsync(TaskModel task)
        {
            // Updates the task in the database
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();  // Saves the changes to the database
        }

        // Deletes a task from the database by its ID
        public async Task DeleteAsync(int id)
        {
            // Finds the task by its ID
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                // If the task is found, remove it from the Tasks DbSet
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();  // Saves the changes to the database
            }
        }

        // Checks if a task exists in the database by its ID
        public async Task<bool> ExistsAsync(int id)
        {
            // Returns true if a task with the specified ID exists, otherwise false
            return await _context.Tasks.AnyAsync(t => t.Id == id);
        }
    }
}
