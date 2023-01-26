using FluentAssertions;
using Moq;
using ShoppingMart.Common.Models;
using ShoppingMart.Common.Repositories;

namespace ShoppingMart.Common.UnitTests.Handlers;

[TestFixture]
public class CartHandlerUnitTests
{
    private Mock<IWareHouseRepository> _repository = null!;
    private Mock<IOrderHandler> _orderHandler = null!;
    private CartHandler _cartHandler = null!;

    private Address _address = null!;
    private Product _product = null!;
    private Discount _discount = null!;
    private Customer _customer = null!;

    [SetUp]
    public void Setup()
    {
        _repository = new Mock<IWareHouseRepository>();
        _orderHandler = new Mock<IOrderHandler>();

        _cartHandler = new CartHandler(_repository.Object, _orderHandler.Object);

        _product = new Product
        {
            Id = 100,
            Price = 5,
            CouponList = new List<Coupon>
            {
               new Coupon (id:1, startDate:DateTime.Today.AddDays(-10), endDate:DateTime.Today.AddDays(+10), discountPercentage:5, name:"5% off")
            }
        };

        _discount = new Discount { DiscountAmount = 2.0M, Tags = new List<string> { "test" } };

        _address = new Address { Line1 = "123 Main st", City = "Siler Town", State = "KY", Zip = "40370" };

        _customer = new(
            fullName: "John Doe",
            email: "john.doe@gmail.com",
            phoneNumber: "213-123-4152",
            mailingAddress: new Address { Line1 = "100 Main st", City = "Raleigh", State = "FL", Zip = "23435" }
        );
    }

    [Test]
    public void CartHandler()
    {
        _cartHandler.Should().NotBeNull();
        _cartHandler.Should().BeOfType(typeof(CartHandler));
    }

    [Test]
    public void CartHandler_Complete_AssignCustomerACart()
    {
        // arrange
        // act
        _cartHandler.AssignCustomerACart(_customer);
        // assert
        _cartHandler.GetCart().Should().NotBeNull();
        _cartHandler.GetCart().Customer.Should().BeSameAs(_customer);
    }

    [Test]
    public void CartHandler_Complete_AddProduct()
    {
        // arrange
        _cartHandler.AssignCustomerACart(_customer);
        _repository.Setup(x => x.CurrentInventory.TryGetValue(It.IsAny<long>(), out _product)).Returns(true);
        // act
        _cartHandler.Add(100, 1);
        // assert
        _cartHandler.GetCart().Should().NotBeNull();
        _cartHandler.GetCart().Customer.Should().BeSameAs(_customer);
        _cartHandler.GetCart().Products.Should().NotBeNullOrEmpty();
        _cartHandler.GetCart().Products[0].Id.Should().Be(100);
        _repository.Verify(x => x.CurrentInventory.TryGetValue(It.IsAny<long>(), out _product), Times.Once);
    }

    [Test]
    public void CartHandler_Complete_AddProduct_False()
    {
        // arrange
        _cartHandler.AssignCustomerACart(_customer);
        _repository.Setup(x => x.CurrentInventory.TryGetValue(It.IsAny<long>(), out _product)).Returns(false);
        // act
        _cartHandler.Add(100, 1);
        // assert
        _cartHandler.GetCart().Should().NotBeNull();
        _cartHandler.GetCart().Customer.Should().BeSameAs(_customer);
        _cartHandler.GetCart().Products.Should().BeEmpty();
        _repository.Verify(x => x.CurrentInventory.TryGetValue(It.IsAny<long>(), out _product), Times.Once);
    }

    [Test]
    public void CartHandler_Complete_Checkout()
    {
        // arrange
        _cartHandler.AssignCustomerACart(_customer);
        _repository.Setup(x => x.CurrentInventory.TryGetValue(It.IsAny<long>(), out _product)).Returns(true);
        _cartHandler.Add(100, 1);
        // act
        _cartHandler.Checkout();
        // assert
        _cartHandler.GetCart().Should().NotBeNull();
        _cartHandler.GetCart().Customer.Should().BeSameAs(_customer);
        _cartHandler.GetCart().Products.Should().NotBeNullOrEmpty();
        _cartHandler.GetCart().Products[0].Id.Should().Be(100);
        _repository.Verify(x => x.CurrentInventory.TryGetValue(It.IsAny<long>(), out _product), Times.Once);
        _orderHandler.Verify(x=> x.PrepareOrder(It.IsAny<Cart>()), Times.Once);

    }
}