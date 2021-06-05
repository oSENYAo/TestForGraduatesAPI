using Microsoft.AspNetCore.Identity;

namespace TestForGraduates.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
    }
}
