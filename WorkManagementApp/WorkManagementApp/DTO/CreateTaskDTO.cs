using WorkManagementApp.Models;
using TaskStatus = WorkManagementApp.Models.TaskStatus;

namespace WorkManagementApp.DTO
{
    public class CreateTaskDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskStatus Status { get; set; } // Beispiel: "In Bearbeitung", "Erledigt"
        public int ProjectId { get; set; }  // ID des Projekts, zu dem die Aufgabe gehört
        public int AssignedUserId { get; set; }  // ID des Benutzers, dem die Aufgabe zugewiesen ist
    }

}
