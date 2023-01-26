using ShoppingMart.Common.Models;

namespace ShoppingMart.Common.Repositories;

public interface ICartHandler
{
    void AssignCustomerACart(Customer customer);
    Cart GetCart();
    void Add(long productId, int qty);
    void Remove(long productId, int qty);
    void Clear();
    Order Checkout();
}

public class CartHandler : ICartHandler
{
    private readonly IWareHouseRepository _wareHouse;
    private readonly IOrderHandler _orderHandler;

    private Cart _cart = null!;
    private Customer _customer = null!;
 
    public CartHandler(IWareHouseRepository wareHouse,
                       IOrderHandler orderHandler)
    {
        this._wareHouse = wareHouse;
        this._orderHandler = orderHandler;
    }
    public void Add(long productId, int qty)
    {
        if (_wareHouse.CurrentInventory.TryGetValue(productId, out var value))
        {
            _cart.Products.Add(value);
            Console.WriteLine($"{value.Name} with Price {value.Price} added to cart.");
            return;
        }
        Console.WriteLine("Product was not found.");
    }

    public void AssignCustomerACart(Customer customer)
    {
        this._customer = customer;
        this._cart = new Cart (customer);
    }

    public Order Checkout()
    {
        return _orderHandler.PrepareOrder(_cart);
    }

    public void Clear()
    {
        _cart.Products?.Clear();
    }

    public Cart GetCart()
    {
        return _cart;
    }

    public void Remove(long productId, int qty)
    {
        var prod = _cart.Products.Find(x => x.Id == productId);
        if(prod != null)
        {
            _cart.Products.Remove(prod);
        }
    }
}
