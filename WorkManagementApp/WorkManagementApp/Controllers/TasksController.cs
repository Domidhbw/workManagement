using Microsoft.AspNetCore.Mvc;
using WorkManagementApp.Models;
using WorkManagementApp.DTOs;
using WorkManagementApp.Services.Tasks;
using TaskModel = WorkManagementApp.Models.Task;
using ProjectModel = WorkManagementApp.Models.Project;
using WorkManagementApp.DTO;
using WorkManagementApp.DTO.Task.WorkManagementApp.DTO.Task;
using TaskStatus = WorkManagementApp.DTO.Task.TaskStatus;
using WorkManagementApp.DTO.Task;

namespace WorkManagementApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        // Konstruktor für den Controller. Der Service wird über Dependency Injection bereitgestellt.
        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        // GET: api/tasks
        // Holt alle Aufgaben und gibt sie als eine Liste von TaskDto zurück.
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
                AssignedUserId = task.AssignedToUserId,
                Priority = task.Priority,
            }).ToList();

            return Ok(taskDtos); // Erfolgreiche Rückgabe als JSON
        }

        // GET: api/tasks/{id}
        // Holt eine Aufgabe anhand ihrer ID und gibt diese als TaskDto zurück.
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound(); // Gibt NotFound zurück, falls die Aufgabe nicht existiert.
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
                AssignedUserId = task.AssignedToUserId,
                Priority = task.Priority,
            };

            return Ok(taskDto); // Erfolgreiche Rückgabe der Task als JSON
        }

        // GET: api/tasks/user/{userId}
        // Holt alle Aufgaben eines bestimmten Benutzers anhand seiner ID.
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetTasksByUserId(int userId)
        {
            var tasks = await _taskService.GetTasksByUserIdAsync(userId);
            if (tasks == null || !tasks.Any())
            {
                return NotFound(); // Gibt NotFound zurück, wenn keine Aufgaben für den Benutzer gefunden werden.
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
                AssignedUserId = t.AssignedToUserId,
                Priority = t.Priority,
            }).ToList();

            return Ok(taskDtos); // Rückgabe der Aufgaben als JSON
        }

        // GET: api/tasks/project/{projectId}
        // Holt alle Aufgaben eines bestimmten Projekts anhand seiner ID.
        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetTasksByProjectId(int projectId)
        {
            var tasks = await _taskService.GetTasksByProjectIdAsync(projectId);
            if (tasks == null || !tasks.Any())
            {
                return Ok(); 
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
                AssignedUserId = t.AssignedToUserId,
                Priority = t.Priority,
            }).ToList();

            return Ok(taskDtos); // Rückgabe der Aufgaben als JSON
        }

        // POST: api/tasks
        // Erstellt eine neue Aufgabe basierend auf den Daten im CreateTaskDto.
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto createTaskDto)
        {
            // Validierung der Enum-Werte (Priority und Status)
            if (!Enum.IsDefined(typeof(Priority), createTaskDto.Priority))
            {
                return BadRequest("Ungültige Priorität.");
            }

            if (!Enum.IsDefined(typeof(TaskStatus), createTaskDto.Status))
            {
                return BadRequest("Ungültiger Status.");
            }

            // Validierung, ob das DueDate in der Zukunft liegt
            if (createTaskDto.DueDate <= DateTime.Now)
            {
                return BadRequest("Das Fälligkeitsdatum muss in der Zukunft liegen.");
            }

            // Validierung, ob das Model korrekt ist
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Mapping des CreateTaskDto zu TaskModel
            var task = new TaskModel
            {
                Title = createTaskDto.Title,
                Description = createTaskDto.Description,
                DueDate = createTaskDto.DueDate,
                Status = createTaskDto.Status,
                ProjectId = createTaskDto.ProjectId,
                AssignedToUserId = createTaskDto.AssignedUserId,
                Priority = createTaskDto.Priority
            };

            // Aufruf des Services, um die Aufgabe zu erstellen
            await _taskService.CreateTaskAsync(task);

            // Mapping des TaskModel zu TaskDto, um die ID nach der Erstellung zurückzugeben
            var taskDtoResponse = new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Status = task.Status,
                ProjectId = task.ProjectId,
                AssignedUserId = task.AssignedToUserId,
                Priority = task.Priority
            };

            return CreatedAtAction(nameof(GetById), new { id = task.Id }, taskDtoResponse);
        }

        // PUT: api/tasks/{id}
        // Aktualisiert eine bestehende Aufgabe anhand ihrer ID mit den neuen Daten aus dem TaskDto.
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TaskDto updatedTaskDto)
        {
            if (id != updatedTaskDto.Id)
            {
                return BadRequest(); // Wenn die IDs nicht übereinstimmen, wird eine BadRequest-Antwort zurückgegeben.
            }

            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound(); // Gibt NotFound zurück, wenn die Aufgabe nicht existiert.
            }

            // Validierung der Enum-Werte (Priority und Status)
            if (!Enum.IsDefined(typeof(Priority), updatedTaskDto.Priority))
            {
                return BadRequest("Ungültige Priorität.");
            }

            if (!Enum.IsDefined(typeof(TaskStatus), updatedTaskDto.Status))
            {
                return BadRequest("Ungültiger Status.");
            }

            // Validierung, ob das DueDate in der Zukunft liegt
            if (updatedTaskDto.DueDate <= DateTime.Now)
            {
                return BadRequest("Das Fälligkeitsdatum muss in der Zukunft liegen.");
            }

            // Update der TaskModel basierend auf den neuen Daten aus TaskDto
            task.Title = updatedTaskDto.Title;
            task.Description = updatedTaskDto.Description;
            task.DueDate = updatedTaskDto.DueDate;
            task.Status = updatedTaskDto.Status;
            task.ProjectId = updatedTaskDto.ProjectId;
            task.AssignedToUserId = updatedTaskDto.AssignedUserId;
            task.Priority = updatedTaskDto.Priority;

            // Aufruf des Services, um die Aufgabe zu aktualisieren
            await _taskService.UpdateTaskAsync(task);
            return NoContent(); // Erfolgreiche Antwort ohne Inhalt
        }

        // DELETE: api/tasks/{id}
        // Löscht eine Aufgabe anhand ihrer ID.
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound(); // Gibt NotFound zurück, wenn die Aufgabe nicht existiert.
            }

            // Aufruf des Services, um die Aufgabe zu löschen
            await _taskService.DeleteTaskAsync(id);
            return NoContent(); // Erfolgreiche Antwort ohne Inhalt
        }

        // POST: api/tasks/{taskId}/comments
        [HttpPost("{taskId}/comments")]
        public async Task<IActionResult> AddComment(int taskId, [FromBody] CreateCommentDto createCommentDto)
        {
            // Überprüfen, ob die Aufgabe existiert
            var task = await _taskService.GetTaskByIdAsync(taskId);
            if (task == null)
            {
                return NotFound("Task not found.");
            }

            // Optional: DTO-Validierung
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Mapping des CreateCommentDto zu TaskComment
            var taskComment = new TaskComment
            {
                CommentText = createCommentDto.CommentText,
                CreatedAt = DateTime.Now,
                TaskId = taskId,
                UserId = createCommentDto.UserId // Der UserId wird hier aus dem DTO übernommen
            };

            // Kommentar zur Aufgabe hinzufügen
            await _taskService.AddCommentToTaskAsync(taskComment);

            // Mapping des TaskComment zu CommentDto
            var commentDto = new CommentDto
            {
                Id = taskComment.Id,
                CommentText = taskComment.CommentText,
                CreatedAt = taskComment.CreatedAt,
                UserId = taskComment.UserId
            };

            // Rückgabe der Antwort mit der neu erstellten Ressource (nur das CommentDto)
            return CreatedAtAction(nameof(GetCommentsByTaskId), new { taskId = taskId }, commentDto);
        }



        // GET: api/tasks/{taskId}/comments
        [HttpGet("{taskId}/comments")]
        public async Task<IActionResult> GetCommentsByTaskId(int taskId)
        {
            // Holen Sie sich alle Kommentare für die angegebene Aufgabe
            var comments = await _taskService.GetCommentsForTaskAsync(taskId);
            if (comments == null || !comments.Any())
            {
                return NotFound();
            }

            // Mapping der Kommentare auf CommentDto
            var commentDtos = comments.Select(comment => new CommentDto
            {
                Id = comment.Id,
                CommentText = comment.CommentText,
                CreatedAt = comment.CreatedAt,
                UserId = comment.UserId
            }).ToList();

            return Ok(commentDtos); // Rückgabe der Kommentare als List
        }



        // DELETE: api/tasks/{taskId}/comments/{commentId}
        [HttpDelete("{taskId}/comments/{commentId}")]
        public async Task<IActionResult> DeleteComment(int taskId, int commentId)
        {
            // Überprüfen, ob die Aufgabe existiert
            var task = await _taskService.GetTaskByIdAsync(taskId);
            if (task == null)
            {
                return NotFound("Task not found.");
            }

            // Löschen des Kommentars
            var comment = await _taskService.GetCommentsForTaskAsync(taskId);
            if (comment == null)
            {
                return NotFound("Comment not found.");
            }

            // Lösche den Kommentar
            await _taskService.DeleteCommentFromTaskAsync(commentId);

            // Erfolgreiche Antwort ohne Inhalt
            return NoContent();
        }




    }
}
