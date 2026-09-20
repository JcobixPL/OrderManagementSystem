using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Api.Contracts.Inventory;
using OrderManagement.Modules.Inventory.Application.Features.Inventory.Create;

namespace OrderManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class InventoryController : ControllerBase
{
    private readonly ISender _sender;

    public InventoryController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        CreateInventoryItemRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateInventoryItemCommand(
            request.ProductId,
            request.InitialQuantity);

        var inventoryItemId = await _sender.Send(
            command,
            cancellationToken);

        return Ok(inventoryItemId);
    }
}
