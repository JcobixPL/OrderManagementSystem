using FluentValidation;

namespace OrderManagement.Modules.Orders.Application.Features.Orders.Create;

internal sealed class CreateOrderCommandValidator
    : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage("Customer ID cannot be empty.");

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("Order must contain at least one item.");

        RuleForEach(x => x.Items)
            .ChildRules(item =>
            {
                item.RuleFor(x => x.ProductId)
                    .NotEmpty()
                    .WithMessage("Product ID cannot be empty.");

                item.RuleFor(x => x.Quantity)
                    .GreaterThan(0)
                    .WithMessage("Quantity must be greater than zero.");
            });
    }
}