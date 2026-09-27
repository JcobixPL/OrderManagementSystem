using MediatR;

namespace OrderManagement.Modules.Orders.Application.Features.Orders.MarkAsDelivered;

public sealed record MarkOrderAsDeliveredCommand(Guid OrderId) : IRequest;