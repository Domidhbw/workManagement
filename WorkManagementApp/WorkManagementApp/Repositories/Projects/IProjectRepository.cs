using WorkManagementApp.Models;

namespace WorkManagementApp.Repositories.Projects
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<IEnumerable<Project>> GetProjectsByUserIdAsync(int userId);
    }
}
