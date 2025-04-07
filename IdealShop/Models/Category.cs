using System.ComponentModel.DataAnnotations;

namespace IdealShop.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        // Navigation Property
        public List<Product> Products { get; set; } = new();
    }

}
