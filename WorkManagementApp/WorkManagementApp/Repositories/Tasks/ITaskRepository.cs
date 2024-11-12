using TaskModel = WorkManagementApp.Models.Task;
namespace WorkManagementApp.Repositories.Tasks
{
    public interface ITaskRepository : IRepository<TaskModel>
    {
        Task<IEnumerable<TaskModel>> GetTasksByUserIdAsync(int userId);
        Task<IEnumerable<TaskModel>> GetTasksByProjectIdAsync(int userId);
    }

}
