using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication6.Models
{
    public class OrderDetail
    {
        [Key]
        public int ID { get; set; }
        public decimal PriceAtPurchase { get; set; }

        [ForeignKey("Order")]
        public int OrderNumber { get; set; }
        public virtual Order Order { get; set; }

        [ForeignKey("Product")]
        public int ProductID { get; set; }
        public virtual Product Product { get; set; }

        public int Quantity { get; set; }
    }
}
