using System;
using System.Collections.Generic;
using System.Text;

namespace OrderManagement.Modules.Orders.Domain.Enums;

public enum OrderStatus
{
    AwaitingPayment = 1,
    Paid = 2,
    Processing = 3,
    ReadyForShipment = 4,
    Shipped = 5,
    Delivered = 6,
    Cancelled = 7,
    Expired = 8
}
