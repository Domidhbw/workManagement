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

        // Constructor: Initializes the TaskService with the TaskRepository and TaskCommentRepository
        public TaskService(ITaskRepository taskRepository, ITaskCommentRepository taskCommentRepository)
        {
            _taskRepository = taskRepository;
            _taskCommentRepository = taskCommentRepository; // Initialize the comment repository
        }

        // Retrieves all tasks
        public async Task<IEnumerable<TaskModel>> GetAllTasksAsync()
        {
            return await _taskRepository.GetAllAsync();
        }

        // Retrieves a specific task by its ID
        public async Task<TaskModel> GetTaskByIdAsync(int id)
        {
            return await _taskRepository.GetByIdAsync(id);
        }

        // Retrieves all tasks assigned to a specific user by their ID
        public async Task<IEnumerable<TaskModel>> GetTasksByUserIdAsync(int userId)
        {
            return await _taskRepository.GetTasksByUserIdAsync(userId);
        }

        // Retrieves all tasks within a specific project by project ID
        public async Task<IEnumerable<TaskModel>> GetTasksByProjectIdAsync(int projectId)
        {
            return await _taskRepository.GetTasksByProjectIdAsync(projectId);
        }

        // Creates a new task and saves it in the repository
        public async Task CreateTaskAsync(TaskModel task)
        {
            await _taskRepository.AddAsync(task);
        }

        // Updates an existing task and saves the changes to the repository
        public async Task UpdateTaskAsync(TaskModel task)
        {
            await _taskRepository.UpdateAsync(task);
        }

        // Deletes a task by its ID
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

        // Delete a comment from a task by its ID
        public async Task DeleteCommentFromTaskAsync(int commentId)
        {
            await _taskCommentRepository.DeleteAsync(commentId);
        }
    }
}
