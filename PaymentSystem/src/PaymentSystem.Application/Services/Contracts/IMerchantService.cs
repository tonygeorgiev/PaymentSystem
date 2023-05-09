using PaymentSystem.Application.DTOs;
using PaymentSystem.Domain.Models;

namespace PaymentSystem.Application.Services.Contracts
{
    public interface IMerchantService
    {
        Task<IEnumerable<Merchant>> GetAllMerchantsAsync();
        Task<Merchant> GetMerchantByIdAsync(Guid id);
        Task<Guid> AddMerchantAsync(MerchantCreateDto merchantCreateDto);
        Task UpdateMerchantAsync(Guid id, MerchantUpdateDto merchantUpdateDto);
        Task DeleteMerchantAsync(Merchant merchant);
        Task CreateMerchantsFromCsvAsync(Stream stream);
    }
}
