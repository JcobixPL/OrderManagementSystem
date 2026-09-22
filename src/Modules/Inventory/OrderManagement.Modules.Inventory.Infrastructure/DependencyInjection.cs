using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Modules.Inventory.Application.Abstractions;
using OrderManagement.Modules.Inventory.Application.Features.Inventory.Create;
using OrderManagement.Modules.Inventory.Infrastructure.Persistence;
using OrderManagement.Modules.Inventory.Infrastructure.Queries;
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

        services.AddMediatR(config =>
            config.RegisterServicesFromAssembly(
                typeof(CreateInventoryItemCommand).Assembly));

        services.AddValidatorsFromAssembly(
            typeof(CreateInventoryItemCommand).Assembly,
            includeInternalTypes: true);

        services.AddScoped<IInventoryRepository, InventoryRepository>();

        services.AddScoped<IInventoryUnitOfWork>(
            sp => sp.GetRequiredService<InventoryDbContext>());

        services.AddScoped<IInventoryReadService, InventoryReadService>();

        return services;
    }
}
