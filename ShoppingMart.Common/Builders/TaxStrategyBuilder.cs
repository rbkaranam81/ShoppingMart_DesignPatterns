using ShoppingMart.Common.Models;
using ShoppingMart.Common.Strategies;

namespace ShoppingMart.Common.Repositories;

public interface ITaxStrategyBuilder : IBuilder<ITaxStrategy>
{
    TaxStrategyBuilder MultiplyTaxBy(decimal times);
    TaxStrategyBuilder TaxPriorToDiscounts();
}

public class TaxStrategyBuilder : ITaxStrategyBuilder
{
    private ITaxStrategy _taxStrategy;

    private readonly decimal _price;
    private readonly decimal _taxPercentage;
    private readonly Discount _discount;

    public TaxStrategyBuilder(decimal price, decimal taxPercentage, Discount discount)
    {
        _price = price;
        _taxPercentage = taxPercentage;
        _discount = discount;

        _taxStrategy = new StandardSalesTaxStrategy { Price = DiscountedPrice(), TaxPercentage = taxPercentage };
    }

    public TaxStrategyBuilder MultiplyTaxBy(decimal times)
    {
        _taxStrategy.TaxPercentage *= times;
        return this;
    }

    public TaxStrategyBuilder TaxPriorToDiscounts()
    {
        _taxStrategy.Price = _price;
        return this;
    }

    public ITaxStrategy Build()
    {
        return _taxStrategy;
    }
    private decimal DiscountedPrice()
    {
        return _price > _discount.DiscountAmount ? (_price - _discount.DiscountAmount) : _price;
    }
}
