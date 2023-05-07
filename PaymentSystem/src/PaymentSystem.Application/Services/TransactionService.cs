using PaymentSystem.Application.Services.Contracts;
using PaymentSystem.Domain.Models;
using PaymentSystem.Infrastructure.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Application.Services
{
    public class TransactionService : ITransactionService
    {

        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
        {
            return await _transactionRepository.GetAllAsync();
        }

        public async Task<Transaction> GetTransactionByIdAsync(Guid id)
        {
            return await _transactionRepository.GetByIdAsync(id);
        }

        public async Task AddTransactionAsync(Transaction transaction)
        {
            await _transactionRepository.AddAsync(transaction);
            await _transactionRepository.SaveAsync();
        }

        public async Task UpdateTransactionAsync(Transaction transaction)
        {
            _transactionRepository.Update(transaction);
            await _transactionRepository.SaveAsync();
        }

        public async Task DeleteTransactionAsync(Transaction transaction)
        {
            _transactionRepository.Delete(transaction);
            await _transactionRepository.SaveAsync();
        }
    }
}
