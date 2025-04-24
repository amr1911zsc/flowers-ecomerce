using System.ComponentModel.DataAnnotations;

namespace WebApplication6.Models
{
    public class Category
    {

        [Key]
        public int CateID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }

    }


}

