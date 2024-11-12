using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WorkManagementApp.Models;
using WorkManagementApp.Repositories.Projects;
using WorkManagementApp.Repositories.Tasks;
using WorkManagementApp.Repositories.Users;
using WorkManagementApp.Services;
using WorkManagementApp.Services.Authentification;
using WorkManagementApp.Services.Projects;
using WorkManagementApp.Services.Tasks;
using Task = System.Threading.Tasks.Task;

namespace WorkManagementApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Registrierung der spezifischen Repositories
            builder.Services.AddScoped<ITaskRepository, TaskRepository>();
            builder.Services.AddScoped<IProjectRepository, ProjectRepository>();

            // Registrierung der Services
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ITaskService, TaskService>();
            builder.Services.AddScoped<IProjectService, ProjectService>();

            // Registrierung von Identity
            builder.Services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddRoleManager<RoleManager<Role>>();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            });

            // Weitere Standardkonfigurationen
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Konfiguration der HTTP-Anforderungsverarbeitung
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            // Initialisierung der Rollen und Benutzer
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var userManager = services.GetRequiredService<UserManager<User>>();
                var roleManager = services.GetRequiredService<RoleManager<Role>>();

                // Erstelle Rollen, wenn sie noch nicht existieren
                await InitializeRolesAsync(roleManager);  // Wir warten jetzt auf den asynchronen Aufruf
            }

            app.Run();
        }

        // Methode zum Erstellen von Rollen
        private static async Task InitializeRolesAsync(RoleManager<Role> roleManager)
        {
            var roles = new[] { "Admin", "Projektmanager", "Mitarbeiter" };

            foreach (var role in roles)
            {
                var roleExist = await roleManager.RoleExistsAsync(role);
                if (!roleExist)
                {
                    var result = await roleManager.CreateAsync(new Role(role));
                    if (result.Succeeded)
                    {
                        Console.WriteLine($"Rolle '{role}' erfolgreich erstellt.");
                    }
                    else
                    {
                        Console.WriteLine($"Fehler beim Erstellen der Rolle '{role}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
                else
                {
                    Console.WriteLine($"Rolle '{role}' existiert bereits.");
                }
            }
        }
    }
}
