using Microsoft.EntityFrameworkCore;
using PaymentSystem.Domain.Models;
using PaymentSystem.Infrastructure.Persistance;
using PaymentSystem.Infrastructure.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
        return await context.Transactions.AnyAsync(predicate);
    }
}