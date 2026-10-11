using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Modules.Identity.Application.Abstractions;
using OrderManagement.Modules.Identity.Application.Features.Auth.Register;
using OrderManagement.Modules.Identity.Infrastructure.Persistence;
using OrderManagement.Modules.Identity.Infrastructure.Repositories;
using OrderManagement.Modules.Identity.Infrastructure.Security;

namespace OrderManagement.Modules.Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityModule(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<IdentityDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddMediatR(config =>
            config.RegisterServicesFromAssembly(
                typeof(RegisterCommand).Assembly));

        services.AddValidatorsFromAssembly(
            typeof(RegisterCommand).Assembly,
            includeInternalTypes: true);    

        services.AddScoped<IPasswordHasher, PasswordHasher>();

        services.AddScoped<IIdentityUnitOfWork>(sp =>
            sp.GetRequiredService<IdentityDbContext>());

        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
