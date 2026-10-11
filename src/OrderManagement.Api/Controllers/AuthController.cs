using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Api.Contracts.Identity;
using OrderManagement.Modules.Identity.Application.Features.Auth.Register;

namespace OrderManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterCommand(
            request.Email,
            request.Password);

        var userId = await _sender.Send(command, cancellationToken);

        return Created(
            $"api/users/{userId}",
            new { Id = userId });
    }
}
