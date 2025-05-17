namespace IdealShop.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string FirstName { get; set; }

        [Required, MaxLength(100)]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; } // Used as Username

        [Required, MaxLength(255)]
        public string Password { get; set; }

        [Required, MaxLength(15)]
        public string PhoneNumber { get; set; }

        [Required, MaxLength(255)]
        public string Address { get; set; }

        // Navigation Property
        public List<CartItem> CartItems { get; set; } = new();
        [ValidateNever]
        public string? Salt { get; set; }


    }

}
