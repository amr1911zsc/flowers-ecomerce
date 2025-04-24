using System.ComponentModel.DataAnnotations;

namespace WebApplication6.Dto
{
    public class productdto
    {
        

        [Required]
        public string Name { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Type { get; set; }

        public string Description { get; set; }

        public string Imagelink { get; set; }

        public int CateID { get; set; } // Category ID (to link the product)


      
    }
}
