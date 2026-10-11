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
}
