﻿using AutoMapper;
using PaymentSystem.Application.DTOs;
using PaymentSystem.Application.Services.Contracts;
using PaymentSystem.Domain.Models;
using PaymentSystem.Infrastructure.Repositories.Contracts;

namespace PaymentSystem.Application.Services
{
    public class TransactionService : ITransactionService
    {

        private readonly ITransactionRepository _transactionRepository;
        private readonly IMerchantRepository _merchantRepository;
        private readonly IMapper _mapper;

        public TransactionService(ITransactionRepository transactionRepository, IMerchantRepository merchantRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _merchantRepository = merchantRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
        {
            return await _transactionRepository.GetAllAsync();
        }

        public async Task<Transaction> GetTransactionByIdAsync(Guid id)
        {
            return await _transactionRepository.GetByIdAsync(id);
        }
        public async Task<Guid> AddTransactionAsync(TransactionCreateDto transactionCreateDto)
        {
            var merchant = await _merchantRepository.GetByIdAsync(transactionCreateDto.MerchantId);

            if (merchant == null)
            {
                throw new ArgumentException($"Merchant with ID {transactionCreateDto.MerchantId} not found.");
            }

            var transaction = _mapper.Map<Transaction>(transactionCreateDto);

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

            transaction.Timestamp = DateTime.UtcNow;
            await _transactionRepository.AddAsync(transaction);
            await _transactionRepository.SaveAsync();

            return transaction.Id;
        }

        public async Task UpdateTransactionAsync(Guid id, TransactionUpdateDto transactionUpdateDto)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);

            if (transaction == null)
            {
                throw new ArgumentException($"Transaction with ID {id} not found.");
            }

            transaction.Amount = transactionUpdateDto.Amount;
            transaction.CustomerEmail = transactionUpdateDto.CustomerEmail;
            transaction.CustomerPhone = transactionUpdateDto.CustomerPhone;

            _transactionRepository.Update(transaction);
            await _transactionRepository.SaveAsync();
        }

        public async Task DeleteTransactionAsync(Transaction transaction)
        {
            _transactionRepository.Delete(transaction);
            await _transactionRepository.SaveAsync();
        }

        public async Task DeleteOldTransactions()
        {
            var dateTime = DateTime.UtcNow.AddHours(-1);
            await _transactionRepository.DeleteTransactionsOlderThan(dateTime);
        }
    }
}
