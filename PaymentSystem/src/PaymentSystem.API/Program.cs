using PaymentSystem.Infrastructure;
using PaymentSystem.Application;
using Microsoft.AspNetCore.Hosting;
using Hangfire;
using Microsoft.Extensions.Configuration;
using PaymentSystem.Application.Services.Contracts;
using PaymentSystem.Infrastructure.Options;
using PaymentSystem.API.Middleware;

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
