using FluentValidation;

namespace OrderManagement.Modules.Orders.Application.Features.Orders.StartProcessing;

internal sealed class StartOrderProcessingCommandValidator
    : AbstractValidator<StartOrderProcessingCommand>
{
    public StartOrderProcessingCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage("Order ID cannot be empty.");
    }
}