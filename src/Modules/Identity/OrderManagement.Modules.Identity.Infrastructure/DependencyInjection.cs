using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Modules.Identity.Application.Abstractions;
using OrderManagement.Modules.Identity.Infrastructure.Persistence;
using OrderManagement.Modules.Identity.Infrastructure.Repositories;

namespace OrderManagement.Modules.Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityModule(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<IdentityDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IIdentityUnitOfWork>(sp =>
            sp.GetRequiredService<IdentityDbContext>());

        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
