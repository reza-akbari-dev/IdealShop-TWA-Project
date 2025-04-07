using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IdealShop.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        [Required, ForeignKey("Customer")]
        public int CustomerId { get; set; }

        [Required, ForeignKey("Product")]
        public int ProductId { get; set; }

        [Required, Range(1, 1000)]
        public int Quantity { get; set; }

        // Navigation Properties
        public Customer Customer { get; set; }
        public Product Product { get; set; }
    }

}
