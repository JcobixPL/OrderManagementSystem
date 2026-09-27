using System;
using System.Collections.Generic;
using System.Text;

namespace OrderManagement.Modules.Orders.Application.Exceptions;

public sealed class OrderNotFoundException : Exception
{
    public OrderNotFoundException(Guid orderId)
        : base($"Order '{orderId}' was not found")
    {
        OrderId = orderId;
    }

    public Guid OrderId { get; }
}
