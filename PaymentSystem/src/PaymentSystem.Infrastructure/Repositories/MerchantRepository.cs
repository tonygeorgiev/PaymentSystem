using PaymentSystem.Domain.Models;
using PaymentSystem.Infrastructure.Persistance;
using PaymentSystem.Infrastructure.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Infrastructure.Repositories;
public class MerchantRepository : Repository<Merchant>, IMerchantRepository
{
    public MerchantRepository(PaymentSystemDbContext context) : base(context) { }
}
