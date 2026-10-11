namespace OrderManagement.Modules.Orders.Application.Abstractions;

public interface ICurrentUser
{
    Guid UserId { get; }
}
