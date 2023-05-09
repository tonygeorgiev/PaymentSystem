using PaymentSystem.Domain.Models;

namespace PaymentSystem.Application.DTOs;

public class TransactionCreateDto
{
    public Guid MerchantId { get; set; }
    public decimal Amount { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomerPhone { get; set; }
    public TransactionType TransactionType { get; set; }
}
