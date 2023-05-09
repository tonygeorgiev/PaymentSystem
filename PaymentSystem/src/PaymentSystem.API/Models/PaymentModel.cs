using PaymentSystem.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace PaymentSystem.API.Models
{
    public class PaymentModel
    {
        [Required, Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required, EmailAddress, MaxLength(150)]
        public string CustomerEmail { get; set; }

        [Required, MaxLength(20)]
        [Phone]
        public string CustomerPhone { get; set; }

        [Required]
        public Guid MerchantId { get; set; }

    }
}
