using PaymentSystem.Domain.Models;
using PaymentSystem.Infrastructure.Persistance;
using PaymentSystem.Infrastructure.Repositories.Contracts;

namespace PaymentSystem.Infrastructure.Repositories;
public class MerchantRepository : Repository<Merchant>, IMerchantRepository
{
    public MerchantRepository(PaymentSystemDbContext context) : base(context) { }
}
