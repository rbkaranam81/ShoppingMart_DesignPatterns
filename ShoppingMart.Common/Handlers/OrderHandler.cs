using ShoppingMart.Common.Models;
using ShoppingMart.Common.Strategies;

namespace ShoppingMart.Common.Repositories;

public interface IOrderHandler
{
    Order PrepareOrder(Cart cart);
}

public class OrderHandler : IOrderHandler
{
    private readonly IWareHouseRepository _wareHouseRepository;
    private readonly ITaxStrategyContext _taxStrategyContext;

    public OrderHandler(IWareHouseRepository wareHouseRepository,
        ITaxStrategyContext taxStrategyContext)
    {
        _wareHouseRepository = wareHouseRepository;
        _taxStrategyContext = taxStrategyContext;
    }
    public Order PrepareOrder(Cart cart)
    {
        var lineItems = new List<OrderLineItem>();

        Console.WriteLine("Check out begins");
        Console.WriteLine("Fetch Promotions ");
        var promotions = _wareHouseRepository.Promotions.ToList();

        // Iterate through products
        foreach (var product in cart.Products)
        {
            if (_wareHouseRepository.CurrentInventory.ContainsKey(product.Id))
            {
                var discount = new DiscountBuilder()
                    .Product(product)
                    .Coupons(product.CouponList)
                    .Promotions(promotions)
                    .Build();

                var taxes = _taxStrategyContext
                   .Context(product, cart.Customer.MailingAddress, discount)
                   .Apply();

                var lineItem = new OrderLineItemBuilder()
                    .Product(product)
                    .Taxes(taxes)
                    .Discount(discount)
                    .Build();

                lineItems.Add(lineItem);
            }
        }

        IOrderBuilder orderBuilder = new OrderBuilder()
            .LineItems(lineItems)
            .ShippingAddress(cart.Customer.MailingAddress);

        return orderBuilder.Build();
    }
}
