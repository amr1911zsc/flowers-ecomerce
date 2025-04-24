using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication6.Models
{
    public class CartItem
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("Cart")]
        public int CartID { get; set; }
        public virtual Cart Cart { get; set; }

        [ForeignKey("Product")]
        public int ProductID { get; set; }
        public virtual Product Product { get; set; }

        public int Quantity { get; set; }
    }
}
