using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderManagement.Modules.Products.Application.Features.Products.ChangePrice;

internal sealed class ChangeProductPriceCommandValidator
    : AbstractValidator<ChangeProductPriceCommand>
{
    public ChangeProductPriceCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required.");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Product price must be greater than zero.");

        RuleFor(x => x.Currency)
            .NotEmpty()
            .WithMessage("Currency is required.")
            .Length(3)
            .WithMessage("Currency must contain exactly 3 characters.");
    }
}
