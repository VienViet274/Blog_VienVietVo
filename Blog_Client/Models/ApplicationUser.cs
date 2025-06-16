using Microsoft.AspNetCore.Identity;

namespace Blog_Client.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string Name { get; set; }
    }
}
