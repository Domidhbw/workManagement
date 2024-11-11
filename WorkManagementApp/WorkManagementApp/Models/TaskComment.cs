namespace WorkManagementApp.Models
{
    public class TaskComment
    {
        public int Id { get; set; }
        public string CommentText { get; set; }
        public DateTime CreatedAt { get; set; }

        public int TaskId { get; set; }
        public Task Task { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }

}
