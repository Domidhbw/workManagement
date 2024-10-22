namespace WorkManagementApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public UserRole Role { get; set; }

        // Navigation Properties
        public ICollection<Project> Projects { get; set; }
        public ICollection<Task> AssignedTasks { get; set; }
    }

}
