using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Api.Contracts.Inventory;
using OrderManagement.Modules.Inventory.Application.DTOs;
using OrderManagement.Modules.Inventory.Application.Features.Inventory.AddStock;
using OrderManagement.Modules.Inventory.Application.Features.Inventory.ConfirmRemoval;
using OrderManagement.Modules.Inventory.Application.Features.Inventory.Create;
using OrderManagement.Modules.Inventory.Application.Features.Inventory.GetByProductId;
using OrderManagement.Modules.Inventory.Application.Features.Inventory.ReleaseReservation;
using OrderManagement.Modules.Inventory.Application.Features.Inventory.Reserve;

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

    [HttpGet("{productId:guid}")]
    public async Task<ActionResult<InventoryItemDto>> GetByProductId(
    Guid productId,
    CancellationToken cancellationToken)
    {
        var inventoryItem = await _sender.Send(
            new GetInventoryByProductIdQuery(productId),
            cancellationToken);

        return Ok(inventoryItem);
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

    [HttpPatch("{productId:guid}/stock")]
    public async Task<IActionResult> AddStock(
        Guid productId,
        AddStockRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddStockCommand(
            productId,
            request.Quantity);

        await _sender.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpPatch("{productId:guid}/reserve")]
    public async Task<IActionResult> Reserve(
        Guid productId,
        ReserveStockRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ReserveStockCommand(
            productId,
            request.Quantity);

        await _sender.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpPatch("{productId:guid}/release")]
    public async Task<IActionResult> Release(
        Guid productId,
        ReleaseReservationRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ReleaseReservationCommand(
            productId,
            request.Quantity);

        await _sender.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpPatch("{productId:guid}/confirm-removal")]
    public async Task<IActionResult> ConfirmRemoval(
    Guid productId,
    ConfirmRemovalRequest request,
    CancellationToken cancellationToken)
    {
        var command = new ConfirmRemovalCommand(
            productId,
            request.Quantity);

        await _sender.Send(command, cancellationToken);

        return NoContent();
    }
}
