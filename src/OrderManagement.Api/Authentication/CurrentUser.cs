using OrderManagement.Modules.Orders.Application.Abstractions;
using System.Security.Claims;

namespace OrderManagement.Api.Authentication;

internal sealed class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId
    {
        get
        {
            var user = _httpContextAccessor.HttpContext?.User
                ?? throw new InvalidOperationException(
                    "Current HTTP context is not available.");

            var userIdClaim = user.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? user.FindFirst("sub")?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                throw new InvalidOperationException(
                    "Authenticated user ID is not available");
            }

            return userId;
        }
    }

    public string Email
    {
        get
        {
            var user = GetUser();

            return user.FindFirstValue(ClaimTypes.Email)
                ?? user.FindFirst("email")?.Value
                ?? throw new InvalidOperationException(
                    "Authenticated user email is not available.");
        }
    }

    public string Role
    {
        get
        {
            var user = GetUser();

            return user.FindFirstValue(ClaimTypes.Role)
                ?? throw new InvalidOperationException(
                    "Authenticated user role is not available.");
        }
    }

    private ClaimsPrincipal GetUser()
    {
        return _httpContextAccessor.HttpContext?.User
            ?? throw new InvalidOperationException(
                "Current HTTP context is not available.");
    }
}
