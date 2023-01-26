using ShoppingMart.Common.Models;
using ShoppingMart.Common.Repositories;
using ShoppingMart.Common.Utilities;

namespace ShoppingMart.Common.Strategies;

public interface ITaxStrategyContext
{
    ITaxStrategy Context(Product product, Address address, Discount discount);
}

public class TaxStrategyContext : ITaxStrategyContext
{
    private readonly IWareHouseRepository repository;

    public TaxStrategyContext(IWareHouseRepository repository)
    {
        this.repository = repository;
    }

    public ITaxStrategy Context(Product product, Address address, Discount discount)
    {
        /*
         *  if special state -> prior to discount
         *  if special category and special state -> double tax
         *  
         */

        ITaxStrategyBuilder taxStrategyBuilder = new TaxStrategyBuilder(product.Price,
                                                                              repository.StateAndTaxes[address.State],
                                                                              discount);
        if (product.IsSpecialCategory &&
            address.State.IsStateWithHighTax())
        {
            taxStrategyBuilder.MultiplyTaxBy(2);
        }

        if (address.State.IsStateWithDiscountRules())
        {
            taxStrategyBuilder.TaxPriorToDiscounts();
        }

        return taxStrategyBuilder.Build();
    }
}
