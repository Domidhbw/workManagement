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

        // Constructor: Initializes the service with the IProjectRepository
        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        // Retrieves all projects by calling the corresponding repository method
        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            // Asynchronously gets all projects from the repository
            return await _projectRepository.GetAllAsync();
        }

        // Retrieves a single project by its ID
        public async Task<Project> GetProjectByIdAsync(int id)
        {
            // Asynchronously gets the project by ID from the repository
            return await _projectRepository.GetByIdAsync(id);
        }

        // Creates a new project by calling the repository's AddAsync method
        public async Task CreateProjectAsync(Project project)
        {
            // Asynchronously adds a new project through the repository
            await _projectRepository.AddAsync(project);
        }

        // Updates an existing project by calling the repository's UpdateAsync method
        public async Task UpdateProjectAsync(Project project)
        {
            // Asynchronously updates the project in the repository
            await _projectRepository.UpdateAsync(project);
        }

        // Deletes a project by its ID by calling the repository's DeleteAsync method
        public async Task DeleteProjectAsync(int id)
        {
            // Asynchronously deletes the project by its ID via the repository
            await _projectRepository.DeleteAsync(id);
        }

        // Retrieves projects assigned to a specific user by their ID
        public async Task<IEnumerable<Project>> GetProjectsByUserIdAsync(int userId)
        {
            // Asynchronously gets projects assigned to the specified user from the repository
            return await _projectRepository.GetProjectsByUserIdAsync(userId);
        }
    }
}
