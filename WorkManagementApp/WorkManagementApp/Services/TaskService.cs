using WorkManagementApp.Models;
using WorkManagementApp.Repositories;

namespace WorkManagementApp.Services
{
    public class TaskService : ITaskService
    {
        private readonly IRepository<Task> _taskRepository;

        public TaskService(IRepository<Task> taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<Task>> GetAllTasksAsync()
        {
            return await _taskRepository.GetAllAsync();
        }

        public async Task<Task> GetTaskByIdAsync(int id)
        {
            return await _taskRepository.GetByIdAsync(id);
        }

        public async Task AddTaskAsync(Task task)
        {
            await _taskRepository.AddAsync(task);
        }

        public async Task UpdateTaskAsync(Task task)
        {
            await _taskRepository.UpdateAsync(task);
        }

        public async Task DeleteTaskAsync(int id)
        {
            await _taskRepository.DeleteAsync(id);
        }
    }
}
