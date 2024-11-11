using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace WorkManagementApp.Models
{
    public class User : IdentityUser<int> 
    {
        public Role Role { get; set; }

        public ICollection<Project> Projects { get; set; }
        public ICollection<Task> AssignedTasks { get; set; }
    }
}
