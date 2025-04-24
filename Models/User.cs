using Microsoft.AspNetCore.Identity;

namespace WebApplication6.Models
{
    public class User: IdentityUser
    {
        public string Name { get; set; }
        public string Location { get; set; }
        
       // public string Type { get; set; } // "Admin" أو "User"
       
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        
        
        public virtual Cart Cart { get; set; }
       
    }
}
