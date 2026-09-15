using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Modules.Products.Infrastructure.Persistence;

namespace OrderManagement.Modules.Products.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddProductsModule(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<ProductsDbContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }
}