namespace WorkManagementApp.DTOs
{
    public class ProjectDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int ManagerId { get; set; }  // ID des Projektmanagers (Wird vom Frontend gesetzt)
        public int? AssignedUserId { get; set; }  // Optional: ID des zugewiesenen Benutzers (kann null sein)
    }
}
