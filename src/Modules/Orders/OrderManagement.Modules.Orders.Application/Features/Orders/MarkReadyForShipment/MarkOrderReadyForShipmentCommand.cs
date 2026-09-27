using MediatR;

namespace OrderManagement.Modules.Orders.Application.Features.Orders.MarkReadyForShipment;

public sealed record MarkOrderReadyForShipmentCommand(Guid OrderId) : IRequest;