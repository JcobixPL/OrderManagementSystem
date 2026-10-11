using Microsoft.AspNetCore.Identity;
using OrderManagement.Modules.Identity.Application.Abstractions;
using OrderManagement.Modules.Identity.Domain.Entities;

namespace OrderManagement.Modules.Identity.Infrastructure.Security;

internal sealed class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<User> _passwordHasher = new();

    public string Hash(string password)
    {
        return _passwordHasher.HashPassword(null!, password);
    }

    public bool Verify(string passwordHash, string password)
    {
        var result = _passwordHasher.VerifyHashedPassword(null!, passwordHash, password);

        return result == PasswordVerificationResult.Success;
    }
}
