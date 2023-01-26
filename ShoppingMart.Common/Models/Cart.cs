using ShoppingMart.Common.Repositories;

namespace ShoppingMart.Common.Models;
public class Cart
{
    public Customer Customer { get; private set; }
    public List<Product> Products { get; set; } = new List<Product>();

    public Cart(Customer customer) => Customer = customer;

}
