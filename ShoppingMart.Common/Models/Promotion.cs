namespace ShoppingMart.Common.Models;
public class Promotion
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal DiscountPercentage { get; set; }
    public decimal FlatDiscount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
