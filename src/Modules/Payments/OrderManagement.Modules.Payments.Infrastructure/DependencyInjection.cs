using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Modules.Payments.Application.Abstractions;
using OrderManagement.Modules.Payments.Infrastructure.Persistence;
using OrderManagement.Modules.Payments.Infrastructure.Repositories;

namespace OrderManagement.Modules.Payments.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddPaymentsModule(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<PaymentsDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IPaymentRepository, PaymentRepository>();

        services.AddScoped<IPaymentsUnitOfWork>(
            sp => sp.GetRequiredService<PaymentsDbContext>());

        return services;
    }
}
