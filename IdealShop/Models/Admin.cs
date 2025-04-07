using System.ComponentModel.DataAnnotations;

namespace IdealShop.Models
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string FirstName { get; set; }

        [Required, MaxLength(100)]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MaxLength(255)]
        public string Password { get; set; }

        [Required, MaxLength(15)]
        public string PhoneNumber { get; set; }

        [Required, MaxLength(255)]
        public string Address { get; set; }
        public string Salt { get; set; } = string.Empty;
    }

}
