using ShoppingMart.Common.Models;

namespace ShoppingMart.Common.Repositories;

public interface IWareHouseRepository
{
    IDictionary<long, Product> CurrentInventory { get; }
    IList<Promotion> Promotions { get; }
    IDictionary<string, decimal> StateAndTaxes { get; }
}

public class WareHouseRepository : IWareHouseRepository
{
    private readonly Warehouse _warehouse;
    private readonly Dictionary<string, decimal> _stateAndTaxes;

    public WareHouseRepository()
    {
        var couponList = new List<Coupon>() { new Coupon(id: 1, startDate: DateTime.Today.AddDays(-10), endDate: DateTime.Today.AddDays(+10), discountPercentage: 5, name: "5% off") };
        var promotions = new List<Promotion> { new Promotion { Id = 1, StartDate = DateTime.Today.AddDays(-10), EndDate = DateTime.Today.AddDays(2), DiscountPercentage = 5, Name = "5% off" } };
        
        this._warehouse = new Warehouse
        {
            CurrentInventory = new Dictionary<long, Product>()
            {
                { 100, new Product {Name = "Toothpaste", Price= 20.0M, Id = 100, Description = "Toothpaste"}},
                { 200, new Product {Name = "Tooth Brush", Price= 30.0M, Id = 200, Description = "Brush", CouponList = couponList}},
                { 300, new Product {Name = "Towels", Price= 80.0M, Id = 300, Description = "Towels", IsSpecialCategory= true}},

            },

            Promotions = promotions
        };

        this._stateAndTaxes = new Dictionary<string, decimal>()
        {
            {"NC", 7.5M },
            {"GA", 2.5M },
            {"FL", 5.5M },
            {"NY", 9.5M },
            {"NM", 2.25M },
            {"NV", 3.5M },
        };
    }

    public IDictionary<long, Product> CurrentInventory { get => _warehouse.CurrentInventory; }
    public IList<Promotion> Promotions { get => _warehouse.Promotions; }

    public IDictionary<string, decimal> StateAndTaxes { get => _stateAndTaxes; }
}
