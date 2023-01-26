namespace ShoppingMart.Common.Models;
public class Product
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsSpecialCategory { get; set; }
    public List<Coupon>? CouponList { get; set; }
}
