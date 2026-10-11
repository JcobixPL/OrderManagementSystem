using MediatR;

namespace OrderManagement.Modules.Identity.Application.Features.Auth.Login;

public sealed record LoginCommand(
    string Email, string Password) : IRequest<string>;
