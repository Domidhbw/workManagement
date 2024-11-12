using System.Collections.Generic;
using System.Threading.Tasks;
using WorkManagementApp.Models;
using WorkManagementApp.Repositories.Projects;
using Task = System.Threading.Tasks.Task;

namespace WorkManagementApp.Services.Projects
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            return await _projectRepository.GetAllAsync();
        }

        public async Task<Project> GetProjectByIdAsync(int id)
        {
            return await _projectRepository.GetByIdAsync(id);
        }

        public async Task CreateProjectAsync(Project project)
        {
            await _projectRepository.AddAsync(project);
        }

        public async Task UpdateProjectAsync(Project project)
        {
            await _projectRepository.UpdateAsync(project);
        }

        public async Task DeleteProjectAsync(int id)
        {
            await _projectRepository.DeleteAsync(id);
        }

        // Neue Methode zum Abrufen von Projekten nach Benutzer-ID
        public async Task<IEnumerable<Project>> GetProjectsByUserIdAsync(int userId)
        {
            return await _projectRepository.GetProjectsByUserIdAsync(userId);
        }
    }
}
