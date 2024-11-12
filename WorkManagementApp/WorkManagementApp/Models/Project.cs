namespace WorkManagementApp.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Fremdschlüssel für den Benutzer, der das Projekt verwaltet (Projektmanager)
        public int ManagerId { get; set; }
        public User Manager { get; set; } // Navigationseigenschaft für den Manager

        // Aufgaben, die zu diesem Projekt gehören
        public ICollection<Task> Tasks { get; set; } // Eine Liste von Aufgaben, die diesem Projekt zugeordnet sind

        // Fremdschlüssel für den Benutzer, dem das Projekt zugewiesen ist (optional, je nach Bedarf)
        public int? AssignedUserId { get; set; }  // Kann null sein, wenn das Projekt keinem bestimmten Benutzer zugewiesen ist
        public User AssignedUser { get; set; }   // Optionale Navigationseigenschaft, falls Projekte einem Benutzer zugewiesen werden

    }
}
