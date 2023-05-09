using PaymentSystem.Domain.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace PaymentSystem.Domain.Models;
public class Transaction : Entity<Guid>
{
    public Transaction()
    {
        this.Id = Guid.NewGuid();
        this.Timestamp = DateTime.UtcNow;
    }

    [Required, Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }

    public TransactionStatus Status { get; set; }

    [Required, EmailAddress, MaxLength(150)]
    public string CustomerEmail { get; set; }

    [Required, MaxLength(20)]
    [Phone]
    public string CustomerPhone { get; set; }

    public Guid MerchantId { get; set; }
    public Merchant Merchant { get; set; }
    public TransactionType TransactionType { get; set; }
    public Guid? ReferencedTransactionId { get; set; }
    public Transaction ReferencedTransaction { get; set; }
    public DateTime Timestamp { get; set; }
}