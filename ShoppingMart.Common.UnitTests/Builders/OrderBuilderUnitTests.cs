using FluentAssertions;
using ShoppingMart.Common.Models;
using ShoppingMart.Common.Repositories;

namespace ShoppingMart.Common.UnitTests.Builders;

[TestFixture]
public class OrderBuilderUnitTests
{
    private OrderBuilder _builder = null!;
    private Address _address = null!;
    private List<OrderLineItem> _lineItems = null!;

    Product _product = null!;
    List<TaxLineItem> _taxes = null!;
    Discount _discount = null!;

    [SetUp]
    public void Setup()
    {
        _builder = new OrderBuilder();
        _product = new Product
        {
            Price = 5,
            CouponList = new List<Coupon>
            {
               new Coupon (id:1, startDate:DateTime.Today.AddDays(-10), endDate:DateTime.Today.AddDays(+10), discountPercentage:5, name:"5% off")
            }
        };

        _taxes = new List<TaxLineItem>
        {
            new TaxLineItem { Tax = 7.5M, Description = "7.5%"}
        };

        _discount = new Discount { DiscountAmount = 12.0M, Tags = new List<string> { "test" } };

        _lineItems = new List<OrderLineItem>
        {
            new OrderLineItem { Product = _product, Taxes = _taxes, TotalDiscount = _discount.DiscountAmount }
        };

        _address = new Address { Line1 = "123 Main st", City = "Siler Town", State = "KY", Zip = "40370" };
    }

    [Test]
    public void OrderBuilder()
    {
        _builder.Should().NotBeNull();
        _builder.Should().BeOfType(typeof(OrderBuilder));
    }

    [Test]
    public void OrderBuilder_Complete_Build()
    {
        // arrange
        _builder
            .LineItems(_lineItems)
            .ShippingAddress(_address);
        // act
        var buildData = _builder.Build();
        // assert
        buildData.Should().NotBeNull();
        buildData.OrderLineItems.Should().NotBeNullOrEmpty();
        buildData.ShippingAddress.Should().NotBeNull();
    }
}