using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication6.Models
{
    public class Cart
    {
        [Key]
        public int CartID { get; set; }
        

        [ForeignKey("User")]
        public string UserID { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; }
    }
}
