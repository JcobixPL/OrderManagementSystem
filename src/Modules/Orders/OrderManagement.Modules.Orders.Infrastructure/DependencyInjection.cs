using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using OrderManagement.Modules.Orders.Application.Abstractions;
using OrderManagement.Modules.Orders.Application.Features.Orders.Create;
using OrderManagement.Modules.Orders.Infrastructure.Integrations;
using OrderManagement.Modules.Orders.Infrastructure.Persistence;
using OrderManagement.Modules.Orders.Infrastructure.Repositories;

namespace OrderManagement.Modules.Orders.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddOrdersModule(
        this IServiceCollection services)
    {
        services.AddDbContext<OrdersDbContext>((serviceProvider, options) =>
        {
            var connection =
                serviceProvider.GetRequiredService<NpgsqlConnection>();

            options.UseNpgsql(connection);
        });

        services.AddMediatR(config =>
            config.RegisterServicesFromAssembly(
                typeof(CreateOrderCommand).Assembly));

        services.AddValidatorsFromAssembly(
            typeof(CreateOrderCommand).Assembly,
            includeInternalTypes: true);

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductCatalog, ProductCatalog>();
        services.AddScoped<IInventoryService, InventoryService>();

        services.AddScoped<IOrdersUnitOfWork>(
            sp => sp.GetRequiredService<OrdersDbContext>());

        return services;
    }
}
