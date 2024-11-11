using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WorkManagementApp.Models;
using WorkManagementApp.Repositories;
using WorkManagementApp.Services;
using TaskModel = WorkManagementApp.Models.Task;

namespace WorkManagementApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Registrierung der Datenbankverbindung
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Registrierung der Repositories
            builder.Services.AddScoped<IRepository<TaskModel>, TaskRepository>();
            builder.Services.AddScoped<IRepository<Project>, ProjectRepository>();
            builder.Services.AddScoped<IRepository<User>, UserRepository>();

            // Registrierung der Services
            builder.Services.AddScoped<IUserService, UserService>(); // Hinzufügen von IUserService
            builder.Services.AddScoped<IAuthService, AuthService>(); // Registrierung von AuthService

            // Registrierung von Identity
            builder.Services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

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

            app.Run();
        }
    }
}
