using System;
using System.Collections.Generic;
using System.Text;

namespace OrderManagement.Modules.Products.Application.Exceptions;

public sealed class ProductAlreadyActiveException : Exception
{
    public ProductAlreadyActiveException(Guid productId)
        : base($"Product with ID '{productId}' is already active")
    {
        ProductId = productId;
    }

    public Guid ProductId { get; }
}