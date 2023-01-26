using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShoppingMart.Common.Models;
using ShoppingMart.Common.Repositories;
using ShoppingMart.Common.Strategies;

namespace ShoppingMart;


internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, ShoppingMart !");
        using var host = CreateHostBuilder(args).Build();
        Initialize(host.Services, out ICartHandler cartHandler);

        Customer customer = new(
            fullName: "John Doe",
            email: "john.doe@gmail.com",
            phoneNumber: "213-123-4152",
            mailingAddress: new Address { Line1 = "100 Main st", City = "Raleigh", State = "FL", Zip = "23435" }
        );

        Console.WriteLine($"Customer {customer.FullName} grabbing a cart and adding products ");

        cartHandler.AssignCustomerACart(customer);
        
        cartHandler.Add(100, 1);
        cartHandler.Add(200, 1);
        cartHandler.Add(300, 1);

        var order = cartHandler.Checkout();
        Console.WriteLine("Order is Ready and below is the Order summary: ");

        Console.WriteLine($@"
                Order number        : {order.OrderNumber}
                Date                : {order.OrderDate} 
                PaymentMethod       : {order.PaymentMethod} 
                ShippingAddress     : {order.ShippingAddress?.Line1}, {order.ShippingAddress?.City} {order.ShippingAddress?.State} {order.ShippingAddress?.Zip} 
                No. of Items        : {order.OrderLineItems?.Count}

                Total Discounts     : {order.TotalDiscounts.ToString("C")} 
                Sub Total           : {order.SubTotal.ToString("C")} 
                Tax                 : {order.Tax.ToString("C")}
                Total               : {order.Total.ToString("C")}
                ");
    }

    private static void Initialize(IServiceProvider services, out ICartHandler cartHandler)
    {
        using var serviceScope = services.CreateScope();
        var provider = serviceScope.ServiceProvider;

        cartHandler = provider.GetRequiredService<ICartHandler>();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureServices((_, services) => services
            .AddTransient<ICartHandler, CartHandler>()
            .AddTransient<IOrderHandler, OrderHandler>()
            .AddTransient<ITaxStrategyContext, TaxStrategyContext>()
            .AddTransient<IWareHouseRepository, WareHouseRepository>()
            );
    }
}