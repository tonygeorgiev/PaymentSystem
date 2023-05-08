using System.ComponentModel.DataAnnotations;

namespace PaymentSystem.API.Models
{
    public class TransactionUpdateModel
    {
        [Required, Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        [EmailAddress]
        public string CustomerEmail { get; set; }

        [Required, MaxLength(20)]
        [Phone]
        public string CustomerPhone { get; set; }
    }
}
