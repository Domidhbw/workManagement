using Microsoft.AspNetCore.Mvc;
using WorkManagementApp.Models;
using WorkManagementApp.DTOs;
using WorkManagementApp.Services.Tasks;
using TaskModel = WorkManagementApp.Models.Task;
using ProjectModel = WorkManagementApp.Models.Project;

namespace WorkManagementApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        // GET: api/tasks
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tasks = await _taskService.GetAllTasksAsync();

            // Mapping der Task-Entities auf TaskDto
            var taskDtos = tasks.Select(task => new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Status = task.Status,
                ProjectId = task.ProjectId,
                AssignedUserId = task.AssignedToUserId
            }).ToList();

            return Ok(taskDtos);
        }

        // GET: api/tasks/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            // Mapping der Task-Entity auf TaskDto
            var taskDto = new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Status = task.Status,
                ProjectId = task.ProjectId,
                AssignedUserId = task.AssignedToUserId
            };

            return Ok(taskDto);
        }

        // GET: api/tasks/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetTasksByUserId(int userId)
        {
            var tasks = await _taskService.GetTasksByUserIdAsync(userId);
            if (tasks == null || !tasks.Any())
            {
                return NotFound();
            }

            // Mapping der Task-Entities auf TaskDto
            var taskDtos = tasks.Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DueDate = t.DueDate,
                Status = t.Status,
                ProjectId = t.ProjectId,
                AssignedUserId = t.AssignedToUserId
            }).ToList();

            return Ok(taskDtos);
        }

        // GET: api/tasks/project/{projectId}
        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetTasksByProjectId(int projectId)
        {
            var tasks = await _taskService.GetTasksByProjectIdAsync(projectId);
            if (tasks == null || !tasks.Any())
            {
                return NotFound();
            }

            // Mapping der Task-Entities auf TaskDto
            var taskDtos = tasks.Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DueDate = t.DueDate,
                Status = t.Status,
                ProjectId = t.ProjectId,
                AssignedUserId = t.AssignedToUserId
            }).ToList();

            return Ok(taskDtos);
        }

        // POST: api/tasks
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaskDto taskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Mapping des TaskDto zu TaskModel
            var task = new TaskModel
            {
                Title = taskDto.Title,
                Description = taskDto.Description,
                DueDate = taskDto.DueDate,
                Status = taskDto.Status,
                ProjectId = taskDto.ProjectId,
                AssignedToUserId = taskDto.AssignedUserId
            };

            await _taskService.CreateTaskAsync(task);

            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }

        // PUT: api/tasks/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TaskDto updatedTaskDto)
        {
            if (id != updatedTaskDto.Id)
            {
                return BadRequest();
            }

            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            // Update der TaskModel basierend auf den neuen Daten aus TaskDto
            task.Title = updatedTaskDto.Title;
            task.Description = updatedTaskDto.Description;
            task.DueDate = updatedTaskDto.DueDate;
            task.Status = updatedTaskDto.Status;
            task.ProjectId = updatedTaskDto.ProjectId;
            task.AssignedToUserId = updatedTaskDto.AssignedUserId;

            await _taskService.UpdateTaskAsync(task);
            return NoContent();
        }

        // DELETE: api/tasks/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            await _taskService.DeleteTaskAsync(id);
            return NoContent();
        }
    }
}
