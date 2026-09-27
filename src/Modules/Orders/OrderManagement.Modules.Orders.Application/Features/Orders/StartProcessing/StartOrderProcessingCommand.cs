using MediatR;

namespace OrderManagement.Modules.Orders.Application.Features.Orders.StartProcessing;

public sealed record StartOrderProcessingCommand(Guid OrderId) : IRequest;