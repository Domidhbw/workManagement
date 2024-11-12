using Microsoft.AspNetCore.Mvc;
using WorkManagementApp.Models;
using WorkManagementApp.DTOs;
using WorkManagementApp.Services.Projects;
using ProjectModel = WorkManagementApp.Models.Project;
using WorkManagementApp.DTO;

namespace WorkManagementApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        // GET: api/projects
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var projects = await _projectService.GetAllProjectsAsync();

            // Mapping der Project-Entities auf ProjectDto
            var projectDtos = projects.Select(project => new ProjectDto
            {
                ID = project.Id,
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                ManagerId = project.ManagerId,
                AssignedUserId = project.AssignedUserId
            }).ToList();

            return Ok(projectDtos);
        }

        // GET: api/projects/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            // Mapping der Project-Entity auf ProjectDto
            var projectDto = new ProjectDto
            {
                ID = project.Id,
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                ManagerId = project.ManagerId,
                AssignedUserId = project.AssignedUserId
            };

            return Ok(projectDto);
        }

        // GET: api/projects/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetProjectsByUserId(int userId)
        {
            var projects = await _projectService.GetProjectsByUserIdAsync(userId);
            if (projects == null || !projects.Any())
            {
                return NotFound();
            }

            // Mapping der Project-Entities auf ProjectDto
            var projectDtos = projects.Select(p => new ProjectDto
            {
                ID = p.Id,
                Name = p.Name,
                Description = p.Description,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                ManagerId = p.ManagerId,
                AssignedUserId = p.AssignedUserId
            }).ToList();

            return Ok(projectDtos);
        }

        // POST: api/projects
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProjectDto createProjectDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Mapping des CreateProjectDto zu ProjectModel
            var project = new ProjectModel
            {
                Name = createProjectDto.Name,
                Description = createProjectDto.Description,
                StartDate = createProjectDto.StartDate,
                EndDate = createProjectDto.EndDate,
                ManagerId = createProjectDto.ManagerId,
                AssignedUserId = createProjectDto.AssignedUserId
            };

            await _projectService.CreateProjectAsync(project);

            // Mapping der ProjectModel zu ProjectDto, um das Projekt mit der ID zurückzugeben
            var projectDtoResponse = new ProjectDto
            {
                ID = project.Id,  // ID wird jetzt gesetzt
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                ManagerId = project.ManagerId,
                AssignedUserId = project.AssignedUserId
            };

            return CreatedAtAction(nameof(GetById), new { id = project.Id }, projectDtoResponse);
        }

        // PUT: api/projects/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProjectDto updatedProjectDto)
        {
            if (id != updatedProjectDto.ID)
            {
                return BadRequest();
            }

            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            project.Name = updatedProjectDto.Name;
            project.Description = updatedProjectDto.Description;
            project.StartDate = updatedProjectDto.StartDate;
            project.EndDate = updatedProjectDto.EndDate;
            project.ManagerId = updatedProjectDto.ManagerId;
            project.AssignedUserId = updatedProjectDto.AssignedUserId;

            await _projectService.UpdateProjectAsync(project);
            return NoContent();
        }

        // DELETE: api/projects/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            await _projectService.DeleteProjectAsync(id);
            return NoContent();
        }
    }
}
