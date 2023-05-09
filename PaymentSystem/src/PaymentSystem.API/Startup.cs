using PaymentSystem.Application.Services.Contracts;
using PaymentSystem.Application.Services;
using PaymentSystem.Infrastructure.Persistance;
using PaymentSystem.Infrastructure.Repositories.Contracts;
using PaymentSystem.Infrastructure.Repositories;
using PaymentSystem.API.Middleware;

namespace PaymentSystem.API
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Add DbContext and repositories
            services.AddDbContext<PaymentSystemDbContext>();
            services.AddScoped<IMerchantRepository, MerchantRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            // Add services
            services.AddScoped<IMerchantService, MerchantService>();
            services.AddScoped<ITransactionService, TransactionService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
