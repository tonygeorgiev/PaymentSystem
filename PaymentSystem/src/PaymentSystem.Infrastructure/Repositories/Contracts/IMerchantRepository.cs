using PaymentSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Infrastructure.Repositories.Contracts;
public interface IMerchantRepository : IRepository<Merchant> { }
