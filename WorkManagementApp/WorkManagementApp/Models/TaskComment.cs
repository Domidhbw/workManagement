namespace WorkManagementApp.Models
{
    public class TaskComment
    {
        public int Id { get; set; }
        public string CommentText { get; set; }
        public DateTime CreatedAt { get; set; }

        // Fremdschlüssel zur verknüpften Aufgabe
        public int TaskId { get; set; }
        public Task Task { get; set; }

        // Fremdschlüssel zum Benutzer, der den Kommentar erstellt hat
        public int UserId { get; set; }
        public User User { get; set; }
    }

}
