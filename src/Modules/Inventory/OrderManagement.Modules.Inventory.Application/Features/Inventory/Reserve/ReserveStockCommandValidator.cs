using FluentValidation;

namespace OrderManagement.Modules.Inventory.Application.Features.Inventory.Reserve;

internal sealed class ReserveStockCommandValidator
    : AbstractValidator<ReserveStockCommand>
{
    public ReserveStockCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID cannot be empty.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero.");
    }
}