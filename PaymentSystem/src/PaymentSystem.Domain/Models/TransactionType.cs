namespace PaymentSystem.Domain.Models;
public enum TransactionType
{
    Authorize,
    Charge,
    Refund,
    Reversal
}