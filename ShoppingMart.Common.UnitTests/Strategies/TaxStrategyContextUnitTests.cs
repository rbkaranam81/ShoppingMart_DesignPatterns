using FluentAssertions;
using Moq;
using ShoppingMart.Common.Models;
using ShoppingMart.Common.Repositories;
using ShoppingMart.Common.Strategies;

namespace ShoppingMart.Common.UnitTests.Strategies;

[TestFixture]
public class TaxStrategyContextUnitTests
{
    private Mock<IWareHouseRepository> _repository = null!;
    private TaxStrategyContext _strategyContext = null!;

    private Address _address = null!;
    private Product _product = null!;
    private Discount _discount = null!;

    [SetUp]
    public void Setup()
    {
        _repository = new Mock<IWareHouseRepository>();
        _strategyContext = new TaxStrategyContext(_repository.Object);

        _product = new Product
        {
            Price = 5,
            CouponList = new List<Coupon>
            {
               new Coupon (id:1, startDate:DateTime.Today.AddDays(-10), endDate:DateTime.Today.AddDays(+10), discountPercentage:5, name:"5% off")
            }
        };

        _discount = new Discount { DiscountAmount = 2.0M, Tags = new List<string> { "test" } };

        _address = new Address { Line1 = "123 Main st", City = "Siler Town", State = "KY", Zip = "40370" };
    }

    [Test]
    public void TaxStrategyContext()
    {
        _strategyContext.Should().NotBeNull();
        _strategyContext.Should().BeOfType(typeof(TaxStrategyContext));
    }

    [Test]
    public void TaxStrategyContext_Complete_Context()
    {
        // arrange
        _repository.Setup(x => x.StateAndTaxes[It.IsAny<string>()]).Returns(7.5M);
        // act
        ITaxStrategy taxStrategy = _strategyContext.Context(_product, _address, _discount);
        // assert
        taxStrategy.Should().NotBeNull();
        taxStrategy.Should().BeOfType(typeof(StandardSalesTaxStrategy));
        taxStrategy.Price.Should().Be(3);
        taxStrategy.TaxPercentage.Should().Be(7.5M);
        _repository.Verify(x => x.StateAndTaxes[It.IsAny<string>()], Times.Once);
    }

    [Test]
    public void TaxStrategyContext_Complete_SpecialCategoryAndState_TaxMultiply()
    {
        // arrange
        _repository.Setup(x => x.StateAndTaxes[It.IsAny<string>()]).Returns(7.5M);
        _product.IsSpecialCategory = true;
        _address.State = "CA";
        // act
        ITaxStrategy taxStrategy = _strategyContext.Context(_product, _address, _discount);
        // assert
        taxStrategy.Should().NotBeNull();
        taxStrategy.Should().BeOfType(typeof(StandardSalesTaxStrategy));
        taxStrategy.Price.Should().Be(3);
        taxStrategy.TaxPercentage.Should().Be(15.0M);
        _repository.Verify(x => x.StateAndTaxes[It.IsAny<string>()], Times.Once);

    }

    [Test]
    public void TaxStrategyContext_Complete_SpecialCategory_NoSpecialState()
    {
        // arrange
        _repository.Setup(x => x.StateAndTaxes[It.IsAny<string>()]).Returns(7.5M);
        _product.IsSpecialCategory = true;
        _address.State = "NC";
        // act
        ITaxStrategy taxStrategy = _strategyContext.Context(_product, _address, _discount);
        // assert
        taxStrategy.Should().NotBeNull();
        taxStrategy.Should().BeOfType(typeof(StandardSalesTaxStrategy));
        taxStrategy.Price.Should().Be(3);
        taxStrategy.TaxPercentage.Should().Be(7.5M);
        _repository.Verify(x => x.StateAndTaxes[It.IsAny<string>()], Times.Once);

    }

    [Test]
    public void TaxStrategyContext_Complete_TaxPriorToDiscountsState()
    {
        // arrange
        _repository.Setup(x => x.StateAndTaxes[It.IsAny<string>()]).Returns(7.5M);
        _product.IsSpecialCategory = false;
        _address.State = "NV";
        // act
        ITaxStrategy taxStrategy = _strategyContext.Context(_product, _address, _discount);
        // assert
        taxStrategy.Should().NotBeNull();
        taxStrategy.Should().BeOfType(typeof(StandardSalesTaxStrategy));
        taxStrategy.Price.Should().Be(5);
        taxStrategy.TaxPercentage.Should().Be(7.5M);
        _repository.Verify(x => x.StateAndTaxes[It.IsAny<string>()], Times.Once);

    }

    [Test]
    public void TaxStrategyContext_Complete_TaxPriorToDiscountsState_SpecialCategoryAndState_TaxMultiply()
    {
        // arrange
        _repository.Setup(x => x.StateAndTaxes[It.IsAny<string>()]).Returns(7.5M);
        _product.IsSpecialCategory = true;
        _address.State = "FL";
        // act
        ITaxStrategy taxStrategy = _strategyContext.Context(_product, _address, _discount);
        // assert
        taxStrategy.Should().NotBeNull();
        taxStrategy.Should().BeOfType(typeof(StandardSalesTaxStrategy));
        taxStrategy.Price.Should().Be(5);
        taxStrategy.TaxPercentage.Should().Be(15M);
        _repository.Verify(x => x.StateAndTaxes[It.IsAny<string>()], Times.Once);

    }
}