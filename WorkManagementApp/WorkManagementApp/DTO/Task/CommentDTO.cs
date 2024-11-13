namespace WorkManagementApp.DTO.Task
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string CommentText { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
    }

}
