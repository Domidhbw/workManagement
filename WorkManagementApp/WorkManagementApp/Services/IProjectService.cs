using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectModel = WorkManagementApp.Models.Project;

namespace WorkManagementApp.Services
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectModel>> GetAllProjectsAsync();
        Task<ProjectModel> GetProjectByIdAsync(int id);
        Task CreateProjectAsync(ProjectModel project);
        Task UpdateProjectAsync(ProjectModel project);
        Task DeleteProjectAsync(int id);
    }
}
