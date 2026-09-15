using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Modules.Products.Application.Features.Products.Create;

namespace OrderManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ProductsController : ControllerBase
{
    private readonly ISender _sender;

    public ProductsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        CreateProductCommand command,
        CancellationToken cancellationToken)
    {
        var productId = await _sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = productId }, productId);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        return Ok();
    }
}
