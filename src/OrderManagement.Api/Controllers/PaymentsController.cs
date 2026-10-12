using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Api.Contracts.Payments;
using OrderManagement.Modules.Payments.Application.Features.Payments.Create;

namespace OrderManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class PaymentsController : ControllerBase
{
    private readonly ISender _sender;

    public PaymentsController(ISender sender)
    {
        _sender = sender;
    }

    [Authorize(Roles = "Customer")]
    [HttpPost]
    public async Task<IActionResult> Create(
        CreatePaymentRequest request,
        CancellationToken cancellationToken)
    {
        var paymentId = await _sender.Send(
            new CreatePaymentCommand(request.OrderId),
            cancellationToken);

        return Created(
            $"/api/payments/{paymentId}",
            new { id = paymentId });
    }
}