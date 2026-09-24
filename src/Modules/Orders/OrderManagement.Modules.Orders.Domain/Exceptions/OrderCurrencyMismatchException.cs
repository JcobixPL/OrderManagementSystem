namespace OrderManagement.Modules.Orders.Domain.Exceptions;

public sealed class OrderCurrencyMismatchException : Exception
{
    public OrderCurrencyMismatchException(
        string orderCurrency,
        string itemCurrency)
        : base(
            $"Order currency '{orderCurrency}' does not match item currency '{itemCurrency}'.")
    {
        OrderCurrency = orderCurrency;
        ItemCurrency = itemCurrency;
    }

    public string OrderCurrency { get; }

    public string ItemCurrency { get; }
}