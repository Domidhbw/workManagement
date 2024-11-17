using WorkManagementApp.DTO.Task.WorkManagementApp.DTO.Task;
using TaskStatus = WorkManagementApp.DTO.Task.TaskStatus;

namespace WorkManagementApp.DTO
{
    public class CreateTaskDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskStatus Status { get; set; } 
        public int ProjectId { get; set; } 
        public int AssignedUserId { get; set; }
        public Priority Priority { get; set; }
    }

}
