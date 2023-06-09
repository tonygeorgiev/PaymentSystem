﻿using AutoMapper;
using CsvHelper;
using PaymentSystem.Application.DTOs;
using PaymentSystem.Application.Exceptions;
using PaymentSystem.Application.Services.Contracts;
using PaymentSystem.Domain.Models;
using PaymentSystem.Infrastructure.Repositories.Contracts;
using System.Globalization;

namespace PaymentSystem.Application.Services
{
    public class MerchantService : IMerchantService
    {
        private readonly IMerchantRepository _merchantRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public MerchantService(IMerchantRepository merchantRepository, ITransactionRepository transactionRepository, IMapper mapper)
        {
            _merchantRepository = merchantRepository;
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Merchant>> GetAllMerchantsAsync()
        {
            return await _merchantRepository.GetAllAsync();
        }

        public async Task<Merchant> GetMerchantByIdAsync(Guid id)
        {
            return await _merchantRepository.GetByIdAsync(id);
        }

        public async Task<Guid> AddMerchantAsync(MerchantCreateDto merchantCreateDto)
        {
            var merchant = _mapper.Map<Merchant>(merchantCreateDto);

            await _merchantRepository.AddAsync(merchant);
            await _merchantRepository.SaveAsync();

            return merchant.Id;
        }

        public async Task UpdateMerchantAsync(Guid id, MerchantUpdateDto merchantUpdateDto)
        {
            var merchant = await _merchantRepository.GetByIdAsync(id);

            if (merchant == null)
            {
                throw new EntityNotFoundException($"Merchant with ID {id} not found.");
            }

            merchant.Name = merchantUpdateDto.Name;
            merchant.Description = merchantUpdateDto.Description;
            merchant.Email = merchantUpdateDto.Email;
            merchant.IsActive = merchantUpdateDto.IsActive;

            _merchantRepository.Update(merchant);
            await _merchantRepository.SaveAsync();
        }

        public async Task DeleteMerchantAsync(Merchant merchant)
        {
            var hasRelatedTransactions = await _transactionRepository.AnyAsync(t => t.MerchantId == merchant.Id);

            if (hasRelatedTransactions)
            {
                throw new InvalidOperationException("Cannot delete a merchant with related payment transactions.");
            }

            _merchantRepository.Delete(merchant);
            await _merchantRepository.SaveAsync();
        }

        public async Task CreateMerchantsFromCsvAsync(Stream stream)
        {
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            
            var records = csv.GetRecords<MerchantCreateDto>();
            
            foreach (var record in records)
            {
                var merchant = new Merchant
                {
                    Name = record.Name,
                    Description = record.Description,
                    Email = record.Email,
                    IsActive = record.IsActive
                };
                await _merchantRepository.AddAsync(merchant);
            }

            await _merchantRepository.SaveAsync();
        }
    }
}
