using WorkManagementApp.Repositories.Tasks;
using WorkManagementApp.Models;
using TaskModel = WorkManagementApp.Models.Task;
using Task = System.Threading.Tasks.Task;
using WorkManagementApp.Repositories.Comments;

namespace WorkManagementApp.Services.Tasks
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskCommentRepository _taskCommentRepository; // Inject the comment repository

        public TaskService(ITaskRepository taskRepository, ITaskCommentRepository taskCommentRepository)
        {
            _taskRepository = taskRepository;
            _taskCommentRepository = taskCommentRepository; // Initialize the comment repository
        }

        public async Task<IEnumerable<TaskModel>> GetAllTasksAsync()
        {
            return await _taskRepository.GetAllAsync();
        }

        public async Task<TaskModel> GetTaskByIdAsync(int id)
        {
            return await _taskRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<TaskModel>> GetTasksByUserIdAsync(int userId)
        {
            return await _taskRepository.GetTasksByUserIdAsync(userId);
        }

        public async Task<IEnumerable<TaskModel>> GetTasksByProjectIdAsync(int projectId)
        {
            return await _taskRepository.GetTasksByProjectIdAsync(projectId);
        }

        public async Task CreateTaskAsync(TaskModel task)
        {
            await _taskRepository.AddAsync(task);
        }

        public async Task UpdateTaskAsync(TaskModel task)
        {
            await _taskRepository.UpdateAsync(task);
        }

        public async Task DeleteTaskAsync(int id)
        {
            await _taskRepository.DeleteAsync(id);
        }

        // New methods to handle task comments

        // Add a comment to a task
        public async Task AddCommentToTaskAsync(TaskComment taskComment)
        {
            // You can perform additional checks here, such as ensuring the task exists
            await _taskCommentRepository.AddAsync(taskComment);
        }

        // Get all comments for a specific task
        public async Task<IEnumerable<TaskComment>> GetCommentsForTaskAsync(int taskId)
        {
            return await _taskCommentRepository.GetByTaskIdAsync(taskId);
        }

        // Delete a comment from a task
        public async Task DeleteCommentFromTaskAsync(int commentId)
        {
            await _taskCommentRepository.DeleteAsync(commentId);
        }
    }
}
