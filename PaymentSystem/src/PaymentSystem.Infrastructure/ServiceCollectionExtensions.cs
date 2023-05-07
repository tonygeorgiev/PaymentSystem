using Microsoft.Extensions.DependencyInjection;
using PaymentSystem.Infrastructure.Repositories.Contracts;
using PaymentSystem.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using PaymentSystem.Infrastructure.Persistance;
using PaymentSystem.Infrastructure.Options;
using Microsoft.EntityFrameworkCore;

namespace PaymentSystem.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var dbSetings = configuration.GetSection(nameof(DatabaseSettings)).Value;
            services.AddDbContext<PaymentSystemDbContext>(options =>
                options.UseSqlServer(dbSetings));
            services.AddScoped<IMerchantRepository, MerchantRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            return services;
        }
    }
}
