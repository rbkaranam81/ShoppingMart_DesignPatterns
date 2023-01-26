namespace ShoppingMart.Common.Models;
public class Coupon
{
    public Coupon(long id,
                  decimal discountPercentage,
                  DateTime startDate,
                  DateTime? endDate,
                  string name)
    {
        Id = id;
        DiscountPercentage = discountPercentage;
        StartDate = startDate;
        EndDate = endDate;
        Name = name;
    }

    public long Id { get; private set; }
    public string Name { get; private set; }
    public decimal DiscountPercentage { get; private set; }
    public decimal FlatDiscount { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
}
