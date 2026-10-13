using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Api.Contracts.Payments;
using OrderManagement.Modules.Payments.Application.Features.Payments.CompleteAttempt;
using OrderManagement.Modules.Payments.Application.Features.Payments.Create;
using OrderManagement.Modules.Payments.Application.Features.Payments.CreateAttempt;
using OrderManagement.Modules.Payments.Application.Features.Payments.FailAttempt;

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

    [Authorize(Roles = "Customer")]
    [HttpPost("{paymentId:guid}/attempts")]
    public async Task<IActionResult> CreateAttempt(
        Guid paymentId,
        CancellationToken cancellationToken)
    {
        var attemptId = await _sender.Send(
            new CreatePaymentAttemptCommand(paymentId),
            cancellationToken);

        return Created(
            $"/api/payments/{paymentId}/attempts/{attemptId}",
            new { id = attemptId });
    }

    [Authorize(Roles = "Customer")]
    [HttpPost("{paymentId:guid}/attempts/{attemptId:guid}/fail")]
    public async Task<IActionResult> FailAttempt(
        Guid paymentId,
        Guid attemptId,
        FailPaymentAttemptRequest request,
        CancellationToken cancellationToken)
    {
        await _sender.Send(
            new FailPaymentAttemptCommand(
                paymentId,
                attemptId,
                request.FailureReason),
            cancellationToken);

        return NoContent();
    }

    [Authorize(Roles = "Customer")]
    [HttpPatch("{paymentId:guid}/attempts/{attemptId:guid}/complete")]
    public async Task<IActionResult> CompleteAttempt(
    Guid paymentId,
    Guid attemptId,
    CompletePaymentAttemptRequest request,
    CancellationToken cancellationToken)
    {
        await _sender.Send(
            new CompletePaymentAttemptCommand(
                paymentId,
                attemptId,
                request.ExternalPaymentId),
            cancellationToken);

        return NoContent();
    }
}