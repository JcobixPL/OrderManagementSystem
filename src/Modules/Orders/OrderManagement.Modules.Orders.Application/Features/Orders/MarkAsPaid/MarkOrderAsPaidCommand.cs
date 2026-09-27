using MediatR;

namespace OrderManagement.Modules.Orders.Application.Features.Orders.MarkAsPaid;

public sealed record MarkOrderAsPaidCommand(Guid OrderId)
    : IRequest;
