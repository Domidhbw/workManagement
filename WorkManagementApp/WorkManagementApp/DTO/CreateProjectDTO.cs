namespace WorkManagementApp.DTO
{
    public class CreateProjectDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ManagerId { get; set; }
        public int AssignedUserId { get; set; }
    }

}
