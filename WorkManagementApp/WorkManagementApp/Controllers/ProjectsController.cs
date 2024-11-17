using Microsoft.AspNetCore.Mvc;
using WorkManagementApp.Models;
using WorkManagementApp.DTOs;
using WorkManagementApp.Services.Projects;
using ProjectModel = WorkManagementApp.Models.Project;
using WorkManagementApp.DTO;

namespace WorkManagementApp.Controllers
{
    // Der Controller behandelt alle Anfragen zu Projekten und stellt die entsprechenden API-Endpunkte bereit
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService; // Service zur Verwaltung von Projekten

        // Konstruktor zur Initialisierung des ProjectService
        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        // GET: api/projects
        // Gibt alle Projekte zurück
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var projects = await _projectService.GetAllProjectsAsync();

            // Mapping der Project-Entities auf ProjectDto, um nur benötigte Daten zurückzugeben
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

            return Ok(projectDtos); // Rückgabe der Projektdaten als Liste von Dtos
        }

        // GET: api/projects/{id}
        // Gibt ein einzelnes Projekt anhand der ID zurück
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound(); // Rückgabe einer NotFound-Antwort, wenn das Projekt nicht existiert
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

            return Ok(projectDto); // Rückgabe des Projekts als Dto
        }

        // GET: api/projects/user/{userId}
        // Gibt alle Projekte zurück, die einem bestimmten Benutzer zugewiesen sind
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetProjectsByUserId(int userId)
        {
            var projects = await _projectService.GetProjectsByUserIdAsync(userId);
            if (projects == null || !projects.Any())
            {
                return NotFound(); // Rückgabe einer NotFound-Antwort, wenn keine Projekte gefunden werden
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

            return Ok(projectDtos); // Rückgabe der Liste von Projekten
        }

        // POST: api/projects
        // Erstellt ein neues Projekt
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProjectDto createProjectDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Rückgabe einer BadRequest-Antwort, wenn das Modell ungültig ist
            }

            // Mapping des CreateProjectDto zu einem ProjectModel
            var project = new ProjectModel
            {
                Name = createProjectDto.Name,
                Description = createProjectDto.Description,
                StartDate = createProjectDto.StartDate,
                EndDate = createProjectDto.EndDate,
                ManagerId = createProjectDto.ManagerId,
                AssignedUserId = createProjectDto.AssignedUserId
            };

            // Projekt wird in der Datenbank erstellt
            await _projectService.CreateProjectAsync(project);

            // Mapping des erstellten ProjectModels zu einem ProjectDto, um das Projekt als Antwort zurückzugeben
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

            // Rückgabe der Antwort mit Status 201 (Created) und dem Projekt-Dto
            return CreatedAtAction(nameof(GetById), new { id = project.Id }, projectDtoResponse);
        }

        // PUT: api/projects/{id}
        // Aktualisiert ein bestehendes Projekt
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProjectDto updatedProjectDto)
        {
            if (id != updatedProjectDto.ID)
            {
                return BadRequest(); // Rückgabe einer BadRequest-Antwort, wenn die ID nicht übereinstimmt
            }

            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound(); // Rückgabe einer NotFound-Antwort, wenn das Projekt nicht existiert
            }

            // Aktualisierung der Projektdaten
            project.Name = updatedProjectDto.Name;
            project.Description = updatedProjectDto.Description;
            project.StartDate = updatedProjectDto.StartDate;
            project.EndDate = updatedProjectDto.EndDate;
            project.ManagerId = updatedProjectDto.ManagerId;
            project.AssignedUserId = updatedProjectDto.AssignedUserId;

            // Speichern der Änderungen in der Datenbank
            await _projectService.UpdateProjectAsync(project);
            return NoContent(); // Rückgabe von Status 204 (NoContent), wenn die Aktualisierung erfolgreich war
        }

        // DELETE: api/projects/{id}
        // Löscht ein Projekt anhand der ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound(); // Rückgabe einer NotFound-Antwort, wenn das Projekt nicht existiert
            }

            // Löschen des Projekts aus der Datenbank
            await _projectService.DeleteProjectAsync(id);
            return NoContent(); // Rückgabe von Status 204 (NoContent), wenn das Projekt erfolgreich gelöscht wurde
        }
    }
}
