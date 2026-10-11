using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Api.Authentication;
using OrderManagement.Api.Contracts.Identity;
using OrderManagement.Modules.Identity.Application.Features.Auth.Login;
using OrderManagement.Modules.Identity.Application.Features.Auth.Register;
using OrderManagement.Modules.Orders.Application.Abstractions;

namespace OrderManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly ISender _sender;
    private readonly ICurrentUser _currentUser;

    public AuthController(ISender sender, ICurrentUser currentUser)
    {
        _sender = sender;
        _currentUser = currentUser;
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
    [HttpGet("me")]
    public IActionResult Me()
    {
        return Ok(new
        {
            id = _currentUser.UserId,
            email = _currentUser.Email,
            role = _currentUser.Role
        });
    }
}
