using WorkManagementApp.Repositories.Tasks;
using TaskModel = WorkManagementApp.Models.Task;

namespace WorkManagementApp.Services.Tasks
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<TaskModel>> GetAllTasksAsync()
        {
            return await _taskRepository.GetAllAsync();
        }

        public async Task<TaskModel> GetTaskByIdAsync(int id)
        {
            return await _taskRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<TaskModel>> GetTasksByUserIdAsync(int userId) // Implementierung der neuen Methode
        {
            return await _taskRepository.GetTasksByUserIdAsync(userId); // Delegiert an Repository
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
    }

}
