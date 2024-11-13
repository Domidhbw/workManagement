using WorkManagementApp.Models;
using Task = System.Threading.Tasks.Task;
using TaskModel = WorkManagementApp.Models.Task;

namespace WorkManagementApp.Services.Tasks
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskModel>> GetAllTasksAsync();
        Task<TaskModel> GetTaskByIdAsync(int id);
        Task<IEnumerable<TaskModel>> GetTasksByUserIdAsync(int userId);
        Task<IEnumerable<TaskModel>> GetTasksByProjectIdAsync(int projectId);
        Task CreateTaskAsync(TaskModel task);
        Task UpdateTaskAsync(TaskModel task);
        Task DeleteTaskAsync(int id);
        Task AddCommentToTaskAsync(TaskComment taskComment);
        Task<IEnumerable<TaskComment>> GetCommentsForTaskAsync(int taskId);
        Task DeleteCommentFromTaskAsync(int commentId);
    }

}
