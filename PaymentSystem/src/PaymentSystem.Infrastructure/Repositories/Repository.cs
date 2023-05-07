using Microsoft.EntityFrameworkCore;
using PaymentSystem.Infrastructure.Persistance;
using PaymentSystem.Infrastructure.Repositories.Contracts;

namespace PaymentSystem.Infrastructure.Repositories;
public class Repository<T> : IRepository<T> where T : class
{
    private readonly PaymentSystemDbContext paymentSystemDbContext;
    private readonly DbSet<T> dbSet;
    public Repository(PaymentSystemDbContext paymentSystemDbContext)
    {
        this.paymentSystemDbContext = paymentSystemDbContext;
        this.dbSet = this.paymentSystemDbContext.Set<T>();
    }
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await this.dbSet.ToListAsync();
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        return await this.dbSet.FindAsync(id);
    }

    public async Task AddAsync(T entity)
    {
        await this.dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        this.dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        this.dbSet.Remove(entity);
    }

    public async Task SaveAsync()
    {
        await this.paymentSystemDbContext.SaveChangesAsync();
    }
}