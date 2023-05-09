using PaymentSystem.Domain.Models;
using System.Linq.Expressions;

namespace PaymentSystem.Infrastructure.Repositories.Contracts;
public interface ITransactionRepository : IRepository<Transaction> {

    Task<bool> AnyAsync(Expression<Func<Transaction, bool>> predicate);
    Task DeleteTransactionsOlderThan(DateTime dateTime);
}
