namespace WebApplication6.Dto
{
    public class OrderDto
    {
        public int OrderID { get; set; }
        public decimal TotalPrice { get; set; }
        public string DeliveryLocation { get; set; }
        public string OrderStatus { get; set; }
        public DateTime DateOfOrder { get; set; }
        
        public List<OrderDetailDto> OrderDetails { get; set; }
    }
}
