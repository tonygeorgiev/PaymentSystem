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
            // Check if the transaction has a reference to another transaction
            if (transaction.ReferencedTransactionId.HasValue)
            {
                var referencedTransaction = await _transactionRepository.GetByIdAsync(transaction.ReferencedTransactionId.Value);

                if (referencedTransaction == null || (referencedTransaction.Status != TransactionStatus.Approved && referencedTransaction.Status != TransactionStatus.Refunded))
                {
                    transaction.Status = TransactionStatus.Error;
                }
                else
                {
                    // Handle different transaction types
                    switch (transaction.TransactionType)
                    {
                        case TransactionType.Authorize:
                            // No special handling required
                            break;

                        case TransactionType.Charge:
                            if (referencedTransaction.TransactionType == TransactionType.Authorize)
                            {
                                transaction.Amount = referencedTransaction.Amount;
                                referencedTransaction.Status = TransactionStatus.Approved;
                            }
                            break;

                        case TransactionType.Refund:
                            if (referencedTransaction.TransactionType == TransactionType.Charge && referencedTransaction.Status == TransactionStatus.Approved)
                            {
                                transaction.Amount = referencedTransaction.Amount;
                                referencedTransaction.Status = TransactionStatus.Refunded;
                            }
                            break;

                        case TransactionType.Reversal:
                            if (referencedTransaction.TransactionType == TransactionType.Authorize)
                            {
                                transaction.Amount = 0;
                                referencedTransaction.Status = TransactionStatus.Reversed;
                            }
                            break;

                        default:
                            throw new InvalidOperationException("Invalid transaction type.");
                    }
                }
            }


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
