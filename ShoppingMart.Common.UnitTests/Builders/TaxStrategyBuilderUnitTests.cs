using FluentAssertions;
using ShoppingMart.Common.Models;
using ShoppingMart.Common.Repositories;

namespace ShoppingMart.Common.UnitTests.Builders;

[TestFixture]
public class TaxStrategyBuilderUnitTests
{
    private TaxStrategyBuilder _builder = null!;
    private readonly decimal _price = 100M;
    private readonly decimal _taxPercentage = 10M;
    private Discount _discount;

    [SetUp]
    public void Setup()
    {
        _discount = new Discount { DiscountAmount = 12.0M, Tags = new List<string> { "test" } };

        _builder = new TaxStrategyBuilder(_price, _taxPercentage, _discount);
    }

    [Test]
    public void TaxStrategyBuilder()
    {
        _builder.Should().NotBeNull();
        _builder.Should().BeOfType(typeof(TaxStrategyBuilder));
    }

    [Test]
    public void TaxStrategyBuilder_Complete_Build()
    {
        // arrange
        // act
        var buildData = _builder.Build();
        // assert
        buildData.Should().NotBeNull();
        buildData.TaxPercentage.Should().Be(10);
        buildData.Apply().Should().NotBeNullOrEmpty();
        buildData.Apply()[0].Tax.Should().Be((_price - _discount.DiscountAmount) * 10 / 100);
    }

    [Test]
    public void TaxStrategyBuilder_Complete_MultiplyTaxBy_Build()
    {
        // arrange
        // act
        var buildData = _builder.
            MultiplyTaxBy(2).
            Build();
        // assert
        buildData.Should().NotBeNull();
        buildData.TaxPercentage.Should().Be(20);
        buildData.Apply().Should().NotBeNullOrEmpty();
        buildData.Apply()[0].Tax.Should().Be((_price- _discount.DiscountAmount) * 20 / 100);
    }

    [Test]
    public void TaxStrategyBuilder_Complete_TaxPriorToDiscounts_Build()
    {
        // arrange
        // act
        var buildData = _builder.
            TaxPriorToDiscounts().
            Build();
        // assert
        buildData.Should().NotBeNull();
        buildData.TaxPercentage.Should().Be(10);
        buildData.Apply().Should().NotBeNullOrEmpty();
        buildData.Apply()[0].Tax.Should().Be((_price) * 10 / 100);
    }
}