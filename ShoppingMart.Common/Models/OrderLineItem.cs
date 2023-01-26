namespace ShoppingMart.Common.Models;
public class OrderLineItem
{
    public Product Product { get; set; }
    public decimal TotalDiscount { get; set; }
    public List<TaxLineItem> Taxes { get; set; }
}
