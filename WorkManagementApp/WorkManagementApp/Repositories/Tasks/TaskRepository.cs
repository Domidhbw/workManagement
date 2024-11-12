using Microsoft.EntityFrameworkCore;
using TaskModel = WorkManagementApp.Models.Task;
using System.Threading.Tasks;
using System.Linq;

namespace WorkManagementApp.Repositories.Tasks
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskModel>> GetAllAsync()
        {
            return await _context.Tasks.Include(t => t.AssignedTo).Include(t => t.Project).ToListAsync();
        }

        public async Task<TaskModel> GetByIdAsync(int id)
        {
            return await _context.Tasks.Include(t => t.AssignedTo).Include(t => t.Project).FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<TaskModel>> GetTasksByUserIdAsync(int userId)
        {
            return await _context.Tasks
                .Include(t => t.AssignedTo)
                .Include(t => t.Project)
                .Where(t => t.AssignedToUserId == userId)
                .ToListAsync();
        }

        public async Task AddAsync(TaskModel task)
        {
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TaskModel task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Tasks.AnyAsync(t => t.Id == id);
        }
    }


}
