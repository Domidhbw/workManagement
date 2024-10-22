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

        // Foreign Keys
        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public int AssignedToUserId { get; set; }
        public User AssignedTo { get; set; } // Benutzer, dem die Aufgabe zugewiesen ist

        // Task Comments
        public ICollection<TaskComment> Comments { get; set; }
    }

}
