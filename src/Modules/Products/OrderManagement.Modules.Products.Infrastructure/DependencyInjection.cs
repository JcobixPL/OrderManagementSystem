using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Modules.Products.Application.Abstractions;
using OrderManagement.Modules.Products.Application.Features.Products.Create;
using OrderManagement.Modules.Products.Infrastructure.Persistence;
using OrderManagement.Modules.Products.Infrastructure.Repositories;

namespace OrderManagement.Modules.Products.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddProductsModule(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<ProductsDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddMediatR(config =>
            config.RegisterServicesFromAssembly(
                typeof(CreateProductCommand).Assembly));

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductsUnitOfWork>(sp =>
           sp.GetRequiredService<ProductsDbContext>());

        return services;
    }
}