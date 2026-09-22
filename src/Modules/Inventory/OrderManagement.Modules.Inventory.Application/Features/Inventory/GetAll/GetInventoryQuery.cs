using MediatR;
using OrderManagement.Modules.Inventory.Application.DTOs;

namespace OrderManagement.Modules.Inventory.Application.Features.Inventory.GetAll;

public sealed record GetInventoryQuery(
    InventoryQueryParameters Parameters) : IRequest<PagedResult<InventoryItemDto>>;