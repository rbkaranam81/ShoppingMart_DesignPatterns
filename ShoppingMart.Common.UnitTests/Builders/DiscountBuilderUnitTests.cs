using FluentAssertions;
using ShoppingMart.Common.Models;
using ShoppingMart.Common.Repositories;

namespace ShoppingMart.Common.UnitTests.Builders;

[TestFixture]
public class DiscountBuilderUnitTests
{
    private DiscountBuilder _builder = null!;
    private Product _product = null!;
    private List<Coupon> _coupons = null!;
    private List<Promotion> _promotions = null!;

    [SetUp]
    public void Setup()
    {
        _builder = new DiscountBuilder();

        _product = new Product
        {
            Price = 5,
            CouponList = new List<Coupon>
            {
               new Coupon (id:1, startDate:DateTime.Today.AddDays(-10), endDate:DateTime.Today.AddDays(+10), discountPercentage:5, name:"5% off")
            }
        };
        _coupons = _product.CouponList;
        _promotions = new List<Promotion>
        {
            new Promotion {Id=1, StartDate=DateTime.Today.AddDays(-10), EndDate=DateTime.Today.AddDays(+10), DiscountPercentage=5, Name="5% off" }
        };

    }

    [Test]
    public void DiscountBuilder()
    {
        _builder.Should().NotBeNull();
        _builder.Should().BeOfType(typeof(DiscountBuilder));
    }

    [Test]
    public void DiscountBuilder_Complete_Build()
    {
        // arrange
        _builder
            .Product(_product)
            .Promotions(_promotions)
            .Coupons(_coupons);
        // act
        var buildData = _builder.Build();
        // assert
        buildData.Should().NotBeNull();
        buildData.DiscountAmount.Should().BeGreaterThan(0);
        buildData.Tags.Should().NotBeNullOrEmpty();
    }

    [Test]
    public void DiscountBuilder_Complete_Build_DiscountsGreaterthanPrice()
    {
        // arrange
        var promotions = new List<Promotion>
        {
            new Promotion {Id=1, StartDate=DateTime.Today.AddDays(-10), EndDate=DateTime.Today.AddDays(2), DiscountPercentage=55, Name="5% off" }
        };

        var coupons = new List<Coupon>
            {
               new Coupon (id:1, startDate:DateTime.Today.AddDays(-10), endDate:DateTime.Today.AddDays(2), discountPercentage:60, name:"5% off")
            };

        _builder
            .Product(_product)
            .Promotions(promotions)
            .Coupons(coupons);
        // act
        var buildData = _builder.Build();
        // assert
        buildData.Should().NotBeNull();
        buildData.DiscountAmount.Should().Be(0);
        buildData.Tags.Should().NotBeNullOrEmpty();
    }

    [Test]
    public void DiscountBuilder_Partial_Build_MissingCoupons_Discounts()
    {
        // arrange
       
        _builder
            .Product(_product);
        // act
        var buildData = _builder.Build();
        // assert
        buildData.Should().NotBeNull();
        buildData.DiscountAmount.Should().Be(0);
        buildData.Tags.Should().BeNullOrEmpty();
    }

    [Test]
    public void DiscountBuilder_Complete_Build_WithExpiredCoupons()
    {
        // arrange

        var coupons = new List<Coupon>
            {
               new Coupon (id:1, startDate:DateTime.Today.AddDays(-10), endDate:DateTime.Today.AddDays(-1), discountPercentage:5, name:"5% off")
            };

        _builder
            .Product(_product)
            .Coupons(coupons);
        // act
        var buildData = _builder.Build();
        // assert
        buildData.Should().NotBeNull();
        buildData.DiscountAmount.Should().Be(0);
        buildData.Tags.Should().BeNullOrEmpty();
    }

    [Test]
    public void DiscountBuilder_Complete_Build_WithExpiredPromotions()
    {
        // arrange

        var promotions = new List<Promotion>
        {
            new Promotion {Id=1, StartDate=DateTime.Today.AddDays(-10), EndDate=DateTime.Today.AddDays(-2), DiscountPercentage=5, Name="5% off" }
        };

        _builder
            .Product(_product)
            .Promotions(promotions);
        // act
        var buildData = _builder.Build();
        // assert
        buildData.Should().NotBeNull();
        buildData.DiscountAmount.Should().Be(0);
        buildData.Tags.Should().BeNullOrEmpty();
    }
}