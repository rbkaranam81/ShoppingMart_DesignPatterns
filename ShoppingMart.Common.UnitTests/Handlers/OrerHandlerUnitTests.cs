using FluentAssertions;
using Moq;
using ShoppingMart.Common.Models;
using ShoppingMart.Common.Repositories;
using ShoppingMart.Common.Strategies;

namespace ShoppingMart.Common.UnitTests.Handlers;

[TestFixture]
public class OrerHandlerUnitTests
{
    private Mock<IWareHouseRepository> _repository = null!;
    private Mock<ITaxStrategyContext> _taxStrategyContext = null!;
    private OrderHandler _orderHandler = null!;
    private Product _product = null!;
    private Customer _customer = null!;
    private List<Promotion> _promotions = null!;
    private Cart _cart = null!;

    [SetUp]
    public void Setup()
    {
        _repository = new Mock<IWareHouseRepository>();
        _taxStrategyContext = new Mock<ITaxStrategyContext>();

        _orderHandler = new OrderHandler(_repository.Object, _taxStrategyContext.Object);

        _product = new Product
        {
            Id = 100,
            Price = 5,
            CouponList = new List<Coupon>
            {
               new Coupon (id:1, startDate:DateTime.Today.AddDays(-10), endDate:DateTime.Today.AddDays(+10), discountPercentage:5, name:"5% off")
            }
        };

        _customer = new(
            fullName: "John Doe",
            email: "john.doe@gmail.com",
            phoneNumber: "213-123-4152",
            mailingAddress: new Address { Line1 = "100 Main st", City = "Raleigh", State = "FL", Zip = "23435" }
        );

        _promotions = new List<Promotion> { new Promotion { Id = 1, StartDate = DateTime.Today.AddDays(-10), EndDate = DateTime.Today.AddDays(2), DiscountPercentage = 5, Name = "5% off" } };

        _cart = new Cart(_customer);
        _cart.Products.Add(_product);
    }

    [Test]
    public void OrderHandler()
    {
        _orderHandler.Should().NotBeNull();
        _orderHandler.Should().BeOfType(typeof(OrderHandler));
    }

    [Test]
    public void OrderHandler_Complete_Order()
    {
        // arrange
        _repository.Setup(x => x.Promotions).Returns(_promotions);
        _repository.Setup(x => x.CurrentInventory.ContainsKey(It.IsAny<long>())).Returns(true);
        _taxStrategyContext.Setup(x => x.Context(It.IsAny<Product>(), It.IsAny<Address>(), It.IsAny<Discount>())).Returns(new StandardSalesTaxStrategy { Price = 5M, TaxPercentage = 7.5M });
        // act
        var order = _orderHandler.PrepareOrder(_cart);
        // assert
        order.Should().NotBeNull();
        order.OrderLineItems.Should().NotBeNullOrEmpty();
        order.OrderLineItems.Should().HaveSameCount(_cart.Products);
        _repository.Verify(x => x.Promotions, Times.Once);
        _repository.Verify(x => x.CurrentInventory.ContainsKey(It.IsAny<long>()), Times.Once);
        _taxStrategyContext.Verify(x => x.Context(It.IsAny<Product>(), It.IsAny<Address>(), It.IsAny<Discount>()), Times.Once);
    }
}