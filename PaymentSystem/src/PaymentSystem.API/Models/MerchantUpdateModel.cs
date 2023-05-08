using System.ComponentModel.DataAnnotations;

namespace PaymentSystem.API.Models
{
    public class MerchantUpdateModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public bool IsActive { get; set; }

    }
}