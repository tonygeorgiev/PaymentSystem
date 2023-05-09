using Microsoft.Extensions.DependencyInjection;
using PaymentSystem.Application.Services.Contracts;
using PaymentSystem.Application.Services;

namespace PaymentSystem.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IMerchantService, MerchantService>();

            return services;
        }
    }
}
