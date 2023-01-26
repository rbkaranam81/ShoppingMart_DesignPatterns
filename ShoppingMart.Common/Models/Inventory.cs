using System.Collections.Concurrent;

namespace ShoppingMart.Common.Models;
public class Inventory
{
    public Dictionary<long, Product> CurrentInventory { get; set; } = new Dictionary<long, Product>();
}
