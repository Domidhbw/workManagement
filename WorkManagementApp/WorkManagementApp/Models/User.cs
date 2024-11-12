using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace WorkManagementApp.Models
{
    public class User : IdentityUser<int>
    {
        // Der Benutzer kann mehrere Projekte leiten
        public ICollection<Project> ManagedProjects { get; set; }

        // Aufgaben, die diesem Benutzer zugewiesen sind
        public ICollection<Task> AssignedTasks { get; set; }

        // Kommentare, die der Benutzer zu Aufgaben hinterlassen hat
        public ICollection<TaskComment> TaskComments { get; set; }
    }
}
