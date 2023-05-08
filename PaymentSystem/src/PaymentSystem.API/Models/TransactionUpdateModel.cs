using System.ComponentModel.DataAnnotations;

namespace PaymentSystem.API.Models
{
    public class TransactionUpdateModel
    {
        [Required]
        public decimal Amount { get; set; }

        [Required]
        [EmailAddress]
        public string CustomerEmail { get; set; }

        [Required]
        public string CustomerPhone { get; set; }
    }
}
