using PaymentSystem.Application.DTOs;
using PaymentSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Application.Services.Contracts
{
    public interface ITransactionService
    {
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
        Task<Transaction> GetTransactionByIdAsync(Guid guid);
        Task AddTransactionAsync(TransactionCreateDto transactionCreateDto);
        Task UpdateTransactionAsync(Guid id, TransactionUpdateDto transactionUpdateDto);
        Task DeleteTransactionAsync(Transaction transaction);
    }
}
