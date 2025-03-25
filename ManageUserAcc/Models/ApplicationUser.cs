using Microsoft.AspNetCore.Identity;

namespace ManageUserAcc.Models
{
    
        public class ApplicationUser : IdentityUser
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Role { get; set; }  // Admin, Manager, Employee
        }

    
}
