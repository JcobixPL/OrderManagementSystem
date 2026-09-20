using MediatR;
using OrderManagement.Modules.Inventory.Application.DTOs;

namespace OrderManagement.Modules.Inventory.Application.Features.Inventory.GetByProductId;

public sealed record GetInventoryByProductIdQuery(Guid ProductId) : IRequest<InventoryItemDto>;
