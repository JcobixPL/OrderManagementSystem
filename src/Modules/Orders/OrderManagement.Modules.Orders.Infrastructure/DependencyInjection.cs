using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Modules.Orders.Application.Abstractions;
using OrderManagement.Modules.Orders.Infrastructure.Persistence;
using OrderManagement.Modules.Orders.Infrastructure.Repositories;

namespace OrderManagement.Modules.Orders.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddOrdersModule(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<OrdersDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IOrderRepository, OrderRepository>();

        services.AddScoped<IOrdersUnitOfWork>(sp =>
            sp.GetRequiredService<OrdersDbContext>());

        return services;
    }
}
