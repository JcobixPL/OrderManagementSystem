using OrderManagement.Modules.Identity.Domain.Entities;

namespace OrderManagement.Modules.Identity.Application.Abstractions;

public interface IJwtTokenGenerator
{
    string Generate(User user);
}
