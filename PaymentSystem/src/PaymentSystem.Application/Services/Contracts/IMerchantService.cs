using PaymentSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Application.Services.Contracts
{
    public interface IMerchantService
    {
        Task<IEnumerable<Merchant>> GetAllMerchantsAsync();
        Task<Merchant> GetMerchantByIdAsync(Guid id);
        Task AddMerchantAsync(Merchant merchant);
        Task UpdateMerchantAsync(Merchant merchant);
        Task DeleteMerchantAsync(Merchant merchant);
    }
}
