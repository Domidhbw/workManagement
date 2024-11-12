using TaskModel = WorkManagementApp.Models.Task;

namespace WorkManagementApp.Services.Tasks
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskModel>> GetAllTasksAsync();
        Task<TaskModel> GetTaskByIdAsync(int id);
        Task<IEnumerable<TaskModel>> GetTasksByUserIdAsync(int userId); // Neue Methode hinzufügen
        Task CreateTaskAsync(TaskModel task);
        Task UpdateTaskAsync(TaskModel task);
        Task DeleteTaskAsync(int id);
    }

}
