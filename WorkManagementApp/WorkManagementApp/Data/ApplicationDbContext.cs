using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkManagementApp.Models;
using TaskModel = WorkManagementApp.Models.Task;

public class ApplicationDbContext : IdentityDbContext<User, Role, int>
{
    // Constructor, der die Optionen an die Basis-Identitätskontextklasse übergibt
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // DbSets für alle Modelle: Diese repräsentieren die Tabellen in der Datenbank
    public DbSet<Project> Projects { get; set; }
    public DbSet<TaskModel> Tasks { get; set; }
    public DbSet<TaskComment> TaskComments { get; set; }

    // Konfiguration der Entitätsbeziehungen
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder); // Ruft die OnModelCreating-Methode der Basis-Klasse auf

        // Beziehung zwischen User und Project (Manager)
        builder.Entity<Project>()
            .HasOne(p => p.Manager)  // Ein Projekt hat einen Manager
            .WithMany(u => u.ManagedProjects)  // Ein User kann viele Projekte verwalten
            .HasForeignKey(p => p.ManagerId);  // Der Manager wird über ManagerId referenziert

        // Beziehung zwischen Task und Project
        builder.Entity<TaskModel>()
            .HasOne(t => t.Project)  // Eine Aufgabe gehört zu einem Projekt
            .WithMany(p => p.Tasks)  // Ein Projekt kann viele Aufgaben haben
            .HasForeignKey(t => t.ProjectId);  // Aufgabenreferenz auf ProjectId

        // Beziehung zwischen Task und User (AssignedTo)
        builder.Entity<TaskModel>()
            .HasOne(t => t.AssignedTo)  // Eine Aufgabe hat einen zugewiesenen User
            .WithMany(u => u.AssignedTasks)  // Ein User kann viele Aufgaben zugewiesen bekommen
            .HasForeignKey(t => t.AssignedToUserId);  // Die Zuweisung erfolgt über AssignedToUserId

        // Beziehung zwischen TaskComment und Task
        builder.Entity<TaskComment>()
            .HasOne(c => c.Task)  // Ein Kommentar gehört zu einer Aufgabe
            .WithMany(t => t.Comments)  // Eine Aufgabe kann viele Kommentare haben
            .HasForeignKey(c => c.TaskId);  // Das TaskComment referenziert die Task über TaskId

        // Beziehung zwischen TaskComment und User
        builder.Entity<TaskComment>()
            .HasOne(c => c.User)  // Ein Kommentar gehört zu einem User
            .WithMany(u => u.TaskComments)  // Ein User kann viele Kommentare verfassen
            .HasForeignKey(c => c.UserId);  // Das Kommentar referenziert den User über UserId
    }
}
