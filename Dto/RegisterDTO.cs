using System.ComponentModel.DataAnnotations;

namespace WebApplication6.Dto
{
    public class RegisterDTO
    {
        [Required]
        public string Name { get; set; }

        
        public string Email { get; set; }
        

        [Required, MinLength(6)]
        public string Password { get; set; }

        public string Location { get; set; }

     
    }
}
