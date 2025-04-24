using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication6.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        public decimal TotalPrice { get; set; }
        public string DeliveryLocation { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;

        public DateTime DateOfOrder { get; set; }
        public DateTime DeliveryDate { get; set; }

        [ForeignKey("User")]
        public string UserID { get; set; }
        
        public virtual User User { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
