using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkManagementApp.Models;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TasksController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WorkManagementApp.Models.Task>>> GetTasks()
    {
        return await _context.Tasks.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<WorkManagementApp.Models.Task>> PostTask(WorkManagementApp.Models.Task task)
    {
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTasks), new { id = task.Id }, task);
    }

}
