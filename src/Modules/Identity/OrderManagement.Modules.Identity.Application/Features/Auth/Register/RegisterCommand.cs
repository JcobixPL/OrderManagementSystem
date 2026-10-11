using MediatR;

namespace OrderManagement.Modules.Identity.Application.Features.Auth.Register;

public sealed record RegisterCommand(
    string Email,
    string Password) : IRequest<Guid>;
