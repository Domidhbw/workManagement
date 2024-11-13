using WorkManagementApp.Models;
using Task = System.Threading.Tasks.Task;

namespace WorkManagementApp.Repositories.Comments
{
    public interface ITaskCommentRepository
    {
        // Fügt einen neuen Kommentar zu einer Aufgabe hinzu
        Task AddAsync(TaskComment taskComment);

        // Gibt alle Kommentare zu einer Aufgabe zurück, basierend auf der TaskId
        Task<IEnumerable<TaskComment>> GetByTaskIdAsync(int taskId);

        // Löscht einen Kommentar anhand seiner Id
        Task DeleteAsync(int commentId);
    }
}
