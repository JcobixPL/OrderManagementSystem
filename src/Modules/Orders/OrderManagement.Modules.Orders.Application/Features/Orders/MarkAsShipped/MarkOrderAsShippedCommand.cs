using MediatR;

namespace OrderManagement.Modules.Orders.Application.Features.Orders.MarkAsShipped;

public sealed record MarkOrderAsShippedCommand(Guid OrderId) : IRequest;