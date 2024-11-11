using Microsoft.AspNetCore.Identity;

namespace WorkManagementApp.Models
{
    // Verwende den Standardkonstruktor, der keine Parameter erfordert
    public class Role : IdentityRole<int>
    {

        public Role() : base() { }

        public Role(string roleName) : base(roleName) { }
    }
}
