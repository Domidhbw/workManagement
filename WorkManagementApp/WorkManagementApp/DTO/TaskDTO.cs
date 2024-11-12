using WorkManagementApp.Models;
using TaskStatus = WorkManagementApp.Models.TaskStatus;

namespace WorkManagementApp.DTOs
{
    public class TaskDto
    {
        public int Id { get; set; } 
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskStatus Status { get; set; } // z.B. "In Bearbeitung", "Erledigt"

        public int ProjectId { get; set; }  // ID des Projekts, zu dem diese Aufgabe gehört
        public int AssignedUserId { get; set; }  // ID des Benutzers, dem die Aufgabe zugewiesen ist
    }
}
