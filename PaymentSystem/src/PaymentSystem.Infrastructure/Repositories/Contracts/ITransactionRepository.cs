using PaymentSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Infrastructure.Repositories.Contracts;
public interface ITransactionRepository : IRepository<Transaction> {

    Task<bool> AnyAsync(Expression<Func<Transaction, bool>> predicate);
}
