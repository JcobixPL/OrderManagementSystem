using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Modules.Inventory.Application.Abstractions;
using OrderManagement.Modules.Inventory.Infrastructure.Persistence;
using OrderManagement.Modules.Inventory.Infrastructure.Repositories;

namespace OrderManagement.Modules.Inventory.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInventoryModule(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<InventoryDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IInventoryRepository, InventoryRepository>();

        services.AddScoped<IInventoryUnitOfWork>(
            sp => sp.GetRequiredService<InventoryDbContext>());

        return services;
    }
}
