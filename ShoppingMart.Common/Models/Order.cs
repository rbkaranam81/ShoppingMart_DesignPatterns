namespace ShoppingMart.Common.Models;
public class Order
{
    public Guid OrderNumber { get; set; }
    public DateTime OrderDate { get; set; }
    public string PaymentMethod { get; set; } = "CC";
    public Address? ShippingAddress { get; set; }

    public List<OrderLineItem> OrderLineItems { get; set; } = new List<OrderLineItem>();

    public decimal SubTotal
    {
        get
        {
            return OrderLineItems.Sum(x => (x.Product.Price - x.TotalDiscount));
        }
    }
    public decimal Tax
    {
        get
        {
            return OrderLineItems.Sum(x => x.Taxes.Sum(t => t.Tax));
        }
    }
    public decimal Total
    {
        get
        {
            return SubTotal + Tax;
        }
    }
    public decimal TotalDiscounts
    {
        get
        {
            return OrderLineItems.Sum(x => (x.TotalDiscount));
        }
    }
}
