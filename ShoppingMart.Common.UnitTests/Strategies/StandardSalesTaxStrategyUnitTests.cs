using FluentAssertions;
using ShoppingMart.Common.Strategies;

namespace ShoppingMart.Common.UnitTests.Strategies;

[TestFixture]
public class StandardSalesTaxStrategyUnitTests
{
    private StandardSalesTaxStrategy _strategy = null!;
    private readonly decimal _price = 100M;
    private readonly decimal _taxPercentage = 10M;

    [SetUp]
    public void Setup()
    {
        _strategy = new StandardSalesTaxStrategy { Price = _price, TaxPercentage = _taxPercentage };
    }

    [Test]
    public void StandardSalesTaxStrategy()
    {
        _strategy.Should().NotBeNull();
        _strategy.Should().BeOfType(typeof(StandardSalesTaxStrategy));
    }

    [Test]
    public void StandardSalesTaxStrategy_Complete_Build()
    {
        // arrange
        // act
        var tax = _strategy.Apply();
        // assert
        tax.Should().NotBeNullOrEmpty();
        tax.Should().HaveCount(1);
        tax[0].Tax.Should().Be((_price) * _taxPercentage / 100);
    }
}