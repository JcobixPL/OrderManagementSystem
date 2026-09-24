using OrderManagement.Modules.Orders.Domain.Enums;

namespace OrderManagement.Modules.Orders.Domain.Exceptions;

public sealed class InvalidOrderStatusTransitionException : Exception
{
    public InvalidOrderStatusTransitionException(
        OrderStatus currentStatus,
        string operation)
        : base(
            $"Operation '{operation}' is not allowed when order status is '{currentStatus}'.")
    {
        CurrentStatus = currentStatus;
        Operation = operation;
    }

    public OrderStatus CurrentStatus { get; }

    public string Operation { get; }
}