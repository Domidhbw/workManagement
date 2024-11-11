using Microsoft.EntityFrameworkCore;
using WorkManagementApp.Models;
using System.Threading.Tasks;
using System.Linq;
using Task = System.Threading.Tasks.Task;

namespace WorkManagementApp.Repositories
{
    public class ProjectRepository : IRepository<Project>
    {
        private readonly ApplicationDbContext _context;

        public ProjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            // Include the related Tasks for each Project
            return await _context.Projects
                .Include(p => p.Tasks)
                .ToListAsync();
        }

        public async Task<Project> GetByIdAsync(int id)
        {
            // Include the related Tasks for the specific Project
            return await _context.Projects
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Project project)
        {
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Project project)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }
        }

        // Überprüft, ob ein Projekt mit der angegebenen ID existiert
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Projects.AnyAsync(p => p.Id == id);
        }
    }
}
