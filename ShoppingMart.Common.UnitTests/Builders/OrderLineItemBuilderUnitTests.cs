using FluentAssertions;
using ShoppingMart.Common.Models;
using ShoppingMart.Common.Repositories;

namespace ShoppingMart.Common.UnitTests.Builders;

[TestFixture]
public class OrderLineItemBuilderUnitTests
{
    private OrderLineItemBuilder _builder = null!;
    private Product _product = null!;
    private List<TaxLineItem> _taxes = null!;
    private Discount _discount = null!;

    [SetUp]
    public void Setup()
    {
        _builder = new OrderLineItemBuilder();

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
    }

    [Test]
    public void OrderLineItem()
    {
        _builder.Should().NotBeNull();
        _builder.Should().BeOfType(typeof(OrderLineItemBuilder));
    }

    [Test]
    public void OrderLineItemBuilder_Complete_Build()
    {
        // arrange
        _builder
            .Product(_product)
            .Taxes(_taxes)
            .Discount(_discount);
        // act
        var buildData = _builder.Build();
        // assert
        buildData.Should().NotBeNull();
        buildData.TotalDiscount.Should().Be(12.0M);
        buildData.Taxes.Should().NotBeNullOrEmpty();
    }

    [Test]
    public void OrderLineItemBuilder_Complete_Build_WithNoDiscounts()
    {
        // arrange
        _builder
            .Product(_product)
            .Taxes(_taxes);
        // act
        var buildData = _builder.Build();
        // assert
        buildData.Should().NotBeNull();
        buildData.TotalDiscount.Should().Be(0);
        buildData.Taxes.Should().NotBeNullOrEmpty();
    }

    [Test]
    public void OrderLineItemBuilder_Complete_Build_WithNoTaxes()
    {
        // arrange
        _builder
            .Product(_product)
            .Discount(_discount);
        // act
        var buildData = _builder.Build();
        // assert
        buildData.Should().NotBeNull();
        buildData.TotalDiscount.Should().Be(12.0M);
        buildData.Taxes.Should().BeNullOrEmpty();
    }
}