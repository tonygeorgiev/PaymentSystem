using PaymentSystem.Application.DTOs;
using PaymentSystem.Domain.Models;

namespace PaymentSystem.Application.Services.Contracts
{
    public interface ITransactionService
    {
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
        Task<Transaction> GetTransactionByIdAsync(Guid guid);
        Task<Guid> AddTransactionAsync(TransactionCreateDto transactionCreateDto);
        Task UpdateTransactionAsync(Guid id, TransactionUpdateDto transactionUpdateDto);
        Task DeleteTransactionAsync(Transaction transaction);
        Task DeleteOldTransactions();
    }
}
