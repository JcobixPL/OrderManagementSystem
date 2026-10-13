using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Modules.Identity.Application.Exceptions;
using OrderManagement.Modules.Inventory.Application.Exceptions;
using OrderManagement.Modules.Inventory.Domain.Exceptions;
using OrderManagement.Modules.Orders.Application.Exceptions;
using OrderManagement.Modules.Orders.Domain.Exceptions;
using OrderManagement.Modules.Payments.Application.Exceptions;
using OrderManagement.Modules.Payments.Domain.Exceptions;
using OrderManagement.Modules.Products.Application.Exceptions;

namespace OrderManagement.Api.Exceptions;

internal sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception,
            "An unhandled exception occured.");

        var statusCode = exception switch
        {
            ValidationException =>
                StatusCodes.Status400BadRequest,

            KeyNotFoundException =>
                StatusCodes.Status404NotFound,

            DuplicateProductSkuException =>
                StatusCodes.Status409Conflict,

            ProductNotFoundException =>
                StatusCodes.Status404NotFound,

            ProductAlreadyActiveException =>
                StatusCodes.Status409Conflict,

            ProductAlreadyInactiveException =>
                StatusCodes.Status409Conflict,

            InventoryItemAlreadyExistsException =>
                StatusCodes.Status409Conflict,

            InventoryItemNotFoundException =>
                StatusCodes.Status404NotFound,

            InsufficientStockException =>
                StatusCodes.Status409Conflict,

            InsufficientReservedStockException =>
                StatusCodes.Status409Conflict,

            OrderProductNotFoundException => 
                StatusCodes.Status404NotFound,

            InactiveOrderProductException => 
                StatusCodes.Status409Conflict,

            OrderNotFoundException =>
                StatusCodes.Status404NotFound,

            InvalidOrderStatusTransitionException =>
                StatusCodes.Status409Conflict,

            EmailAlreadyInUseException =>
                StatusCodes.Status409Conflict,

            InvalidCredentialsException =>
                StatusCodes.Status401Unauthorized,

            OrderAccessDeniedException =>
                StatusCodes.Status403Forbidden,

            PaymentAlreadyExistsException =>
                StatusCodes.Status409Conflict,

            OrderNotPayableException =>
                StatusCodes.Status409Conflict,

            PendingPaymentAttemptAlreadyExistsException =>
                StatusCodes.Status409Conflict,

            InvalidPaymentAttemptStatusTransitionException =>
                StatusCodes.Status409Conflict,

            PaymentNotFoundException =>
                StatusCodes.Status404NotFound,

            PaymentAttemptNotFoundException =>
                StatusCodes.Status404NotFound,

            _ =>
                StatusCodes.Status500InternalServerError
        };

        var title = exception switch
        {
            ValidationException =>
                "Validation error",

            KeyNotFoundException =>
                "Resource not found",

            DuplicateProductSkuException =>
                "Conflict",

            ProductNotFoundException =>
                "Resource not found",

            ProductAlreadyInactiveException =>
                "Conflict",

            ProductAlreadyActiveException =>
                "Conflict",

            InventoryItemAlreadyExistsException =>
                "Conflict",

            InventoryItemNotFoundException =>
                "Resource not found",

            InsufficientStockException =>
                "Conflict",

            InsufficientReservedStockException =>
                "Conflict",

            OrderProductNotFoundException =>
                "Product not found",

            InactiveOrderProductException =>
                "Product is inactive",

            OrderNotFoundException =>
                "Order not found",

            InvalidOrderStatusTransitionException =>
                "Invalid order status transition",

            EmailAlreadyInUseException =>
                "Email already in use",

            InvalidCredentialsException =>
                "Invalid credentials",

            OrderAccessDeniedException =>
                "Access denied",

            PaymentAlreadyExistsException =>
                "Payment already exists",

            OrderNotPayableException =>
                "Order cannot be paid",

            PendingPaymentAttemptAlreadyExistsException =>
                "Pending payment attempt already exists",

            InvalidPaymentAttemptStatusTransitionException =>
                "Invalid payment attempt status transition",

            PaymentNotFoundException =>
                "Payment not found",

            PaymentAttemptNotFoundException =>
                "Payment attempt not found",

            _ =>
                "An unexpected error occured"
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = statusCode == StatusCodes.Status500InternalServerError
                ? "An unexpected error occured."
                : exception.Message,
            Instance = httpContext.Request.Path
        };

        if (exception is ValidationException validationException)
        {
            problemDetails.Extensions["errors"] =
                validationException.Errors
                    .GroupBy(error => error.PropertyName)
                    .ToDictionary(
                        group => group.Key,
                        group => group
                            .Select(error => error.ErrorMessage)
                            .ToArray());
        }

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);

        return true;
    }
}
