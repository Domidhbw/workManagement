using Microsoft.EntityFrameworkCore;
using WorkManagementApp.Models;
using WorkManagementApp.Repositories.Comments;
using Task = System.Threading.Tasks.Task;

namespace WorkManagementApp.Repositories.Tasks
{
    public class TaskCommentRepository : ITaskCommentRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskCommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Fügt einen neuen Kommentar hinzu
        public async Task AddAsync(TaskComment taskComment)
        {
            await _context.TaskComments.AddAsync(taskComment);
            await _context.SaveChangesAsync();
        }

        // Gibt alle Kommentare für eine bestimmte Aufgabe zurück
        public async Task<IEnumerable<TaskComment>> GetByTaskIdAsync(int taskId)
        {
            var result = await _context.TaskComments
                .Where(c => c.TaskId == taskId)
                .ToListAsync();
            return result;
        }

        // Löscht einen Kommentar
        public async Task DeleteAsync(int commentId)
        {
            var comment = await _context.TaskComments.FindAsync(commentId);
            if (comment != null)
            {
                _context.TaskComments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }
    }
}
