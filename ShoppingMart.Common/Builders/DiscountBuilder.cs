using ShoppingMart.Common.Models;

namespace ShoppingMart.Common.Repositories;

public interface IDiscountBuilder : IBuilder<Discount>
{
    DiscountBuilder Product(Product product);
    DiscountBuilder Coupons(List<Coupon> coupons);
    DiscountBuilder Promotions(List<Promotion> promotions);
}
public class DiscountBuilder : IDiscountBuilder
{
    private readonly Discount _discount = new()
    {
        DiscountAmount = 0,
        Tags = new List<string>()
    };

    private List<Coupon>? _coupons;
    private List<Promotion>? _promotions;
    private Product? _product;
    public Discount Build()
    {
        var discountAmount = CalculateCouponsTotal() + CalculatePromotionsTotal();
        _discount.DiscountAmount = discountAmount <= _product?.Price ? discountAmount : 0.0M;
        return _discount;
    }

    public DiscountBuilder Product(Product product)
    {
        this._product = product;
        return this;
    }

    public DiscountBuilder Coupons(List<Coupon> coupons)
    {
        this._coupons = coupons;
        return this;
    }

    public DiscountBuilder Promotions(List<Promotion> promotions)
    {
        this._promotions = promotions;
        return this;
    }

    private decimal CalculateCouponsTotal()
    {
        decimal totalDiscount = 0;

        var coupons = _coupons?.Where(p => p.StartDate <= DateTime.Today && p.EndDate >= DateTime.Today).ToList();

        coupons?.ForEach(c =>
        {
            if (c.FlatDiscount > 0)
            {
                totalDiscount += c.FlatDiscount;
            }
            else
            {
                totalDiscount += (_product?.Price * c.DiscountPercentage)/100 ?? 0.0M;
            }
            _discount?.Tags?.Add(c.Name);
        });
        return totalDiscount;
    }

    private decimal CalculatePromotionsTotal()
    {
        decimal totalDiscount = 0;

        var promotions = _promotions?.Where(p => p.StartDate <= DateTime.Today && p.EndDate >= DateTime.Today).ToList();

        promotions?.ForEach(p =>
        {
            if (p.FlatDiscount > 0)
            {
                totalDiscount += p.FlatDiscount;
            }
            else
            {
                totalDiscount += (_product?.Price * p.DiscountPercentage)/100 ?? 0.0M;
            }
            _discount?.Tags?.Add(p.Name);
        });
        return totalDiscount;
    }
}
