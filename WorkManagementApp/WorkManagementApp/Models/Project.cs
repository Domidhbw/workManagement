namespace WorkManagementApp.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public User Manager { get; set; }
        public ICollection<Task> Tasks { get; set; }
    }

}
