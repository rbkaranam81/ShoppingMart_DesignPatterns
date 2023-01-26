using ShoppingMart.Common.Models;

namespace ShoppingMart.Common.Repositories;

public interface IOrderLineItemBuilder : IBuilder<OrderLineItem>
{
    OrderLineItemBuilder Product(Product product);
    OrderLineItemBuilder Taxes(List<TaxLineItem> taxes);
    OrderLineItemBuilder Discount(Discount discount);
}

public class OrderLineItemBuilder : IOrderLineItemBuilder
{
    private readonly OrderLineItem _lineItem = new();

    public OrderLineItem Build()
    {
        return _lineItem;
    }

    public OrderLineItemBuilder Discount(Discount discount)
    {
        _lineItem.TotalDiscount = discount.DiscountAmount;
        return this;
    }

    public OrderLineItemBuilder Product(Product product)
    {
        _lineItem.Product = product;
        return this;
    }

    public OrderLineItemBuilder Taxes(List<TaxLineItem> taxes)
    {
        _lineItem.Taxes = taxes;
        return this;
    }
}
