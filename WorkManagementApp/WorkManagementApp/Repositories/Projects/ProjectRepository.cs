using Microsoft.EntityFrameworkCore;
using WorkManagementApp.Models;
using System.Threading.Tasks;
using System.Linq;
using Task = System.Threading.Tasks.Task;

namespace WorkManagementApp.Repositories.Projects
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _context;

        // Constructor: Initialisiert den Repository mit dem ApplicationDbContext
        public ProjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Methode, um alle Projekte abzurufen (inklusive zugehöriger Aufgaben)
        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            // Include sorgt dafür, dass auch die zugehörigen Aufgaben des Projekts geladen werden
            return await _context.Projects
                .Include(p => p.Tasks) // Enthält die zugehörigen Aufgaben für jedes Projekt
                .ToListAsync(); // Listet alle Projekte mit ihren Aufgaben auf
        }

        // Methode, um ein spezifisches Projekt anhand der ID abzurufen (inklusive zugehöriger Aufgaben)
        public async Task<Project> GetByIdAsync(int id)
        {
            // Include sorgt dafür, dass auch die zugehörigen Aufgaben für das spezifische Projekt geladen werden
            return await _context.Projects
                .Include(p => p.Tasks) // Enthält die zugehörigen Aufgaben für das Projekt
                .FirstOrDefaultAsync(p => p.Id == id); // Sucht nach dem Projekt mit der angegebenen ID
        }

        // Methode, um ein neues Projekt hinzuzufügen
        public async Task AddAsync(Project project)
        {
            // Füge das neue Projekt der DbSet hinzu
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync(); // Speichert die Änderungen in der Datenbank
        }

        // Methode, um ein bestehendes Projekt zu aktualisieren
        public async Task UpdateAsync(Project project)
        {
            // Aktualisiere das Projekt
            _context.Projects.Update(project);
            await _context.SaveChangesAsync(); // Speichert die Änderungen in der Datenbank
        }

        // Methode, um ein Projekt zu löschen
        public async Task DeleteAsync(int id)
        {
            // Sucht das Projekt in der Datenbank
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                // Entfernt das gefundene Projekt
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync(); // Speichert die Änderungen in der Datenbank
            }
        }

        // Methode, die überprüft, ob ein Projekt mit der angegebenen ID existiert
        public async Task<bool> ExistsAsync(int id)
        {
            // Überprüft, ob ein Projekt mit der angegebenen ID in der Datenbank existiert
            return await _context.Projects.AnyAsync(p => p.Id == id);
        }

        // Methode, um alle Projekte abzurufen, die einem bestimmten Benutzer zugewiesen sind
        public async Task<IEnumerable<Project>> GetProjectsByUserIdAsync(int userId)
        {
            // Sucht Projekte, bei denen der Benutzer der zugewiesene Benutzer ist (hier wird angenommen, dass es eine Zuweisung gibt)
            return await _context.Projects
                .Include(p => p.AssignedUser) // Falls es eine Navigationseigenschaft zu User gibt
                .Where(p => p.AssignedUserId == userId) // Filtert nach Projekten, die dem Benutzer zugewiesen sind
                .ToListAsync(); // Gibt die Liste der Projekte zurück, die dem Benutzer zugewiesen sind
        }
    }
}
