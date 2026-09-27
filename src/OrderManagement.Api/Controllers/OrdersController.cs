using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Api.Contracts.Orders;
using OrderManagement.Modules.Orders.Application.Features.Orders.Create;
using OrderManagement.Modules.Orders.Application.Features.Orders.GetById;
using OrderManagement.Modules.Orders.Application.Features.Orders.MarkAsPaid;

namespace OrderManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class OrdersController : ControllerBase
{
    private readonly ISender _sender;

    public OrdersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var order = await _sender.Send(
            new GetOrderByIdQuery(id),
            cancellationToken);

        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateOrderRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateOrderCommand(
            request.CustomerId,
            request.Items
                .Select(item => new CreateOrderItem(
                    item.ProductId,
                    item.Quantity))
                .ToList());

        var orderId = await _sender.Send(
            command,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = orderId },
            new { id = orderId });
    }

    [HttpPatch("{id:guid}/mark-paid")]
    public async Task<IActionResult> MarkAsPaid(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _sender.Send(
            new MarkOrderAsPaidCommand(id),
            cancellationToken);

        return NoContent();
    }
}
