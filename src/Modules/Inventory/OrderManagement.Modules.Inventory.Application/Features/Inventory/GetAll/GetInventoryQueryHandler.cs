using MediatR;
using OrderManagement.Modules.Inventory.Application.Abstractions;
using OrderManagement.Modules.Inventory.Application.DTOs;

namespace OrderManagement.Modules.Inventory.Application.Features.Inventory.GetAll;

internal sealed class GetInventoryQueryHandler
    : IRequestHandler<GetInventoryQuery, PagedResult<InventoryItemDto>>
{
    private readonly IInventoryReadService _readService;

    public GetInventoryQueryHandler(IInventoryReadService readService)
    {
        _readService = readService;
    }

    public Task<PagedResult<InventoryItemDto>> Handle(
        GetInventoryQuery request,
        CancellationToken cancellationToken)
    {
        return _readService.GetPagedAsync(
            request.Parameters,
            cancellationToken);
    }
}
