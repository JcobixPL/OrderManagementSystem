using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderManagement.Modules.Inventory.Application.Features.Inventory.ReleaseReservation;

public sealed record ReleaseReservationCommand(
    Guid ProductId,
    int Quantity) : IRequest;