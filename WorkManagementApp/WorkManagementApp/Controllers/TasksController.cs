using Microsoft.AspNetCore.Mvc;
using TaskModel = WorkManagementApp.Models.Task;
using WorkManagementApp.Repositories;

namespace WorkManagementApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly IRepository<TaskModel> _taskRepository;

        public TasksController(IRepository<TaskModel> taskRepository)
        {
            _taskRepository = taskRepository;
        }

        // GET: api/tasks
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tasks = await _taskRepository.GetAllAsync();
            return Ok(tasks);
        }

        // GET: api/tasks/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        // POST: api/tasks
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaskModel task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _taskRepository.AddAsync(task);
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }

        // PUT: api/tasks/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TaskModel updatedTask)
        {
            if (id != updatedTask.Id)
            {
                return BadRequest();
            }

            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            await _taskRepository.UpdateAsync(updatedTask);
            return NoContent();
        }

        // DELETE: api/tasks/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            await _taskRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
