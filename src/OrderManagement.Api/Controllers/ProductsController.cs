using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Api.Contracts.Products;
using OrderManagement.Modules.Products.Application.DTOs;
using OrderManagement.Modules.Products.Application.Features.Products.ChangePrice;
using OrderManagement.Modules.Products.Application.Features.Products.Create;
using OrderManagement.Modules.Products.Application.Features.Products.GetAll;
using OrderManagement.Modules.Products.Application.Features.Products.GetById;

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

    [HttpGet]
    public async Task<ActionResult<PagedResult<ProductDto>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] string? sortDirection = null,
        CancellationToken cancellationToken = default)
    {
        var parameters = new ProductQueryParameters(
            pageNumber,
            pageSize,
            search,
            isActive,
            sortBy,
            sortDirection);

        var result = await _sender.Send(
            new GetProductsQuery(parameters),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductDto>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var product = await _sender.Send(
            new GetProductByIdQuery(id),
            cancellationToken);

        if (product is null)
            return NotFound();

        return Ok(product);
    }

    [HttpPatch("{id:guid}/price")]
    public async Task<IActionResult> ChangePrice(
        Guid id,
        ChangeProductPriceRequest request,
        CancellationToken cancellationToken)
    {
        await _sender.Send(
            new ChangeProductPriceCommand(
                id,
                request.Price,
                request.Currency),
            cancellationToken);

        return NoContent();
    }
}
