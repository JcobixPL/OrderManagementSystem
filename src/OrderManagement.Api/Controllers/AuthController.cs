using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Api.Contracts.Identity;
using OrderManagement.Modules.Identity.Application.Features.Auth.Login;
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

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LoginCommand(
            request.Email,
            request.Password);

        var token = await _sender.Send(command, cancellationToken);

        return Ok(new
        {
            accessToken = token
        });
    }

    [Authorize]
    [HttpGet("me-test")]
    public IActionResult MeTest()
    {
        return Ok(new
        {
            message = "You are authenticated."
        });
    }
}
