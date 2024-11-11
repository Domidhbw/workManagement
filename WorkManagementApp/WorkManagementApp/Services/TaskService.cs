using WorkManagementApp.Repositories;
using TaskModel = WorkManagementApp.Models.Task;

namespace WorkManagementApp.Services
{
    public class TaskService : ITaskService
    {
        private readonly IRepository<TaskModel> _taskRepository;

        public TaskService(IRepository<TaskModel> taskRepository)
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
