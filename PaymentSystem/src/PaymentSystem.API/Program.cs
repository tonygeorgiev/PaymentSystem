using PaymentSystem.Infrastructure;
using PaymentSystem.Application;
using Hangfire;
using PaymentSystem.Application.Services.Contracts;
using PaymentSystem.Infrastructure.Options;
using PaymentSystem.API.Middleware;
using Microsoft.AspNetCore.TestHost;
using PaymentSystem.API;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var services = builder.Services;
        var configuration = builder.Configuration;


        // Add services to the container.
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddInfrastructure(configuration);
        services.AddApplication();
        services.AddAutoMapper(typeof(Program));
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder =>
                {
                    builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
        });
        services.AddHangfire(config =>
        {
            config.UseSqlServerStorage(configuration.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>().DefaultConnection);
        });

        var app = builder.Build();

        app.UseMiddleware<ExceptionHandlerMiddleware>();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHangfireDashboard();
        app.UseHangfireServer();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.UseCors("AllowAllOrigins");

        using (var server = new BackgroundJobServer())
        {
            RecurringJob.AddOrUpdate<ITransactionService>(
                "DeleteOldTransactions",
                service => service.DeleteOldTransactions(),
                Cron.Hourly);
        }
        app.Run();
    }
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });

    public static TestServer GetTestServer()
    {
        var hostBuilder = CreateHostBuilder(null);
        var testServer = new TestServer(new WebHostBuilder().UseStartup<Startup>());

        return testServer;
    }

    public static HttpClient GetTestClient()
    {
        var testServer = GetTestServer();
        var client = testServer.CreateClient();

        return client;
    }
}