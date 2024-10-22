using WorkManagementApp.Models;

namespace WorkManagementApp.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<Task>> GetAllTasksAsync();
        Task<Task> GetTaskByIdAsync(int id);
        Task AddTaskAsync(Task task);
        Task UpdateTaskAsync(Task task);
        Task DeleteTaskAsync(int id);
    }
}
