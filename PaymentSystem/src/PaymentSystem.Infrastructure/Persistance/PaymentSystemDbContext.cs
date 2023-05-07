using Microsoft.EntityFrameworkCore;
using PaymentSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Transaction = PaymentSystem.Domain.Models.Transaction;

namespace PaymentSystem.Infrastructure.Persistance;
public class PaymentSystemDbContext : DbContext
{
    public PaymentSystemDbContext(DbContextOptions<PaymentSystemDbContext> options)
        : base(options)
    {
    }

    public DbSet<Merchant> Merchants { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
}