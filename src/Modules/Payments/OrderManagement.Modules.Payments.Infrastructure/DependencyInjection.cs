using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Modules.Payments.Application.Abstractions;
using OrderManagement.Modules.Payments.Application.Features.Payments.Create;
using OrderManagement.Modules.Payments.Infrastructure.Integrations;
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

        services.AddMediatR(config =>
            config.RegisterServicesFromAssembly(
                typeof(CreatePaymentCommand).Assembly));

        services.AddValidatorsFromAssembly(
            typeof(CreatePaymentCommand).Assembly,
            includeInternalTypes: true);

        services.AddScoped<IPaymentRepository, PaymentRepository>();

        services.AddScoped<IOrderPaymentDetailsProvider, OrderPaymentDetailsProvider>();

        services.AddScoped<IOrderPaymentStatusUpdater, OrderPaymentStatusUpdater>();

        services.AddScoped<IPaymentsUnitOfWork>(
            sp => sp.GetRequiredService<PaymentsDbContext>());

        return services;
    }
}
