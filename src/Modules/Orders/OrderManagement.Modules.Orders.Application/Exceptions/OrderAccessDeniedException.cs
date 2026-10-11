namespace OrderManagement.Modules.Orders.Application.Exceptions;

public sealed class OrderAccessDeniedException : Exception
{
    public OrderAccessDeniedException()
        : base("You do not have access to this order.")
    {
    }
}