namespace ShoppingMart.Common.Models;
public class Warehouse
{
    public Dictionary<long, Product> CurrentInventory { get; set; } = new Dictionary<long, Product>();
    public IList<Promotion>? Promotions { get; set; } = new List<Promotion>();
}
