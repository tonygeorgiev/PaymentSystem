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
    public class MerchantService : IMerchantService
    {
        private readonly IMerchantRepository _merchantRepository;

        public MerchantService(IMerchantRepository merchantRepository)
        {
            _merchantRepository = merchantRepository;
        }

        public async Task<IEnumerable<Merchant>> GetAllMerchantsAsync()
        {
            return await _merchantRepository.GetAllAsync();
        }

        public async Task<Merchant> GetMerchantByIdAsync(Guid id)
        {
            return await _merchantRepository.GetByIdAsync(id);
        }

        public async Task AddMerchantAsync(Merchant merchant)
        {
            await _merchantRepository.AddAsync(merchant);
            await _merchantRepository.SaveAsync();
        }

        public async Task UpdateMerchantAsync(Merchant merchant)
        {
            _merchantRepository.Update(merchant);
            await _merchantRepository.SaveAsync();
        }

        public async Task DeleteMerchantAsync(Merchant merchant)
        {
            _merchantRepository.Delete(merchant);
            await _merchantRepository.SaveAsync();
        }
    }
}
