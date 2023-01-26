using ShoppingMart.Common.Models;

namespace ShoppingMart.Common.Repositories;

public interface IOrderBuilder : IBuilder<Order>
{
    OrderBuilder LineItems(List<OrderLineItem> lineItems);
    OrderBuilder ShippingAddress(Address address);
}

public class OrderBuilder : IOrderBuilder
{
    private readonly Order _order = new()
    {
        OrderDate = DateTime.Now,
        OrderNumber = Guid.NewGuid(),
        PaymentMethod = "CC",
    };

    public OrderBuilder LineItems(List<OrderLineItem> lineItems)
    {
        _order.OrderLineItems = lineItems;
        return this;
    }
    public OrderBuilder ShippingAddress(Address address)
    {
        _order.ShippingAddress = address;
        return this;
    }

    public Order Build()
    {
        return _order;
    }
}
