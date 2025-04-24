using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication6.Models
{
    public class Product
    {

        public int ProductID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Type { get; set; }
        public string imglink {  get; set; }

        public string Description { get; set; }

        [Required]

        [ForeignKey("Category")]
        public int? CateID { get; set; }

        public  virtual Category? Category { get; set; } // Make Category optional



        public virtual ICollection<CartItem> CartItems { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}

