using Microsoft.EntityFrameworkCore;
using OrderManagement.Modules.Identity.Application.Abstractions;
using OrderManagement.Modules.Identity.Domain.Entities;
using OrderManagement.Modules.Identity.Infrastructure.Persistence;

namespace OrderManagement.Modules.Identity.Infrastructure.Repositories;

internal sealed class UserRepository : IUserRepository
{
    private readonly IdentityDbContext _dbContext;

    public UserRepository(IdentityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _dbContext.Users.AddAsync(user, cancellationToken);
    }

    public Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();

        return _dbContext.Users
            .AnyAsync(
                x => x.Email == normalizedEmail,
                cancellationToken);
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();

        return _dbContext.Users
            .SingleOrDefaultAsync(
                x => x.Email == normalizedEmail,
                cancellationToken);
    }
}
