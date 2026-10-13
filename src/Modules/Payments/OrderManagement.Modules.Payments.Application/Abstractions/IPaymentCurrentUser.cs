namespace OrderManagement.Modules.Payments.Application.Abstractions;

public interface IPaymentCurrentUser
{
    Guid UserId { get; }
}