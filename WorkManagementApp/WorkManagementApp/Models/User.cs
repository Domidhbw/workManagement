using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace WorkManagementApp.Models
{
    // Erbt von IdentityUser<int>, um die ID als int zu definieren
    public class User : IdentityUser<int>  // Achtung: Die ID ist jetzt vom Typ int
    {
        // Benutzerdefinierte Felder hinzufügen
        public Role Role { get; set; }

        // Navigation Properties (Beziehungen zu anderen Entitäten)
        public ICollection<Project> Projects { get; set; }
        public ICollection<Task> AssignedTasks { get; set; }
    }
}
