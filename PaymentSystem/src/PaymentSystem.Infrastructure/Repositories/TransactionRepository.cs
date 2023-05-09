using Microsoft.EntityFrameworkCore;
using PaymentSystem.Domain.Models;
using PaymentSystem.Infrastructure.Persistance;
using PaymentSystem.Infrastructure.Repositories.Contracts;
using System.Linq.Expressions;

namespace PaymentSystem.Infrastructure.Repositories;
public class TransactionRepository : Repository<Transaction>, ITransactionRepository
{
    private readonly PaymentSystemDbContext context;

    public TransactionRepository(PaymentSystemDbContext context) : base(context)
    {
        this.context = context;
    }
    public async Task<bool> AnyAsync(Expression<Func<Transaction, bool>> predicate)
    {
        return await this.context.Transactions.AnyAsync(predicate);
    }
    public async Task DeleteTransactionsOlderThan(DateTime dateTime)
    {
        await this.context.Transactions.Where(t => t.Timestamp < dateTime).ForEachAsync(t => this.context.Remove(t));
        await this.context.SaveChangesAsync();
    }

}