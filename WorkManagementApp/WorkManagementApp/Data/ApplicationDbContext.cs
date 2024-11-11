using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkManagementApp.Models;
using TaskModel = WorkManagementApp.Models.Task;

public class ApplicationDbContext : IdentityDbContext<User, Role, int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Project> Projects { get; set; }
    public DbSet<TaskModel> Tasks { get; set; }
    public DbSet<TaskComment> TaskComments { get; set; }
}
