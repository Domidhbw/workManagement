namespace WorkManagementApp.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskStatus Status { get; set; }
        public int Priority { get; set; } // 1 = Hoch, 2 = Mittel, 3 = Niedrig

        // Fremdschlüssel zum verknüpften Projekt
        public int ProjectId { get; set; }
        public Project Project { get; set; }

        // Fremdschlüssel zum Benutzer, dem die Aufgabe zugewiesen ist
        public int AssignedToUserId { get; set; }
        public User AssignedTo { get; set; }

        // Kommentare zu dieser Aufgabe
        public ICollection<TaskComment> Comments { get; set; }
    }

}
