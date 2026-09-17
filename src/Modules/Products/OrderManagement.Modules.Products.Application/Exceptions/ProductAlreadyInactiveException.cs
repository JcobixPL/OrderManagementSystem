using System;
using System.Collections.Generic;
using System.Text;

namespace OrderManagement.Modules.Products.Application.Exceptions;

public sealed class ProductAlreadyInactiveException : Exception
{
    public ProductAlreadyInactiveException(Guid productId)
        : base($"Product with ID '{productId}' is already inactive")
    {
        ProductId = productId;
    }

    public Guid ProductId { get; }
}