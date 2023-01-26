using ShoppingMart.Common.Models;

namespace ShoppingMart.Common.Strategies;

public interface ITaxStrategy
{
    List<TaxLineItem> Apply();
    decimal Price { get; set; }
    decimal TaxPercentage { get; set; }
}

public class StandardSalesTaxStrategy : ITaxStrategy
{
    private decimal taxPercentage;
    private decimal price;

    public decimal Price { get => price; set => price = value; }
    public decimal TaxPercentage { get => taxPercentage; set => taxPercentage = value > 0 ? value : 0; }

    public List<TaxLineItem> Apply() => new() { new TaxLineItem { Description = $"Sales tax at {TaxPercentage}%", Tax = Price * (TaxPercentage / 100) } };
}

// You can use this to add additional tax line items
// Can use as Decorator if needed
public class MultiTaxStrategy : ITaxStrategy
{
    private readonly ITaxStrategy taxCalculator;

    public MultiTaxStrategy(ITaxStrategy taxCalculator)
    {
        this.taxCalculator = taxCalculator;
    }
    public decimal Price { get; set; }
    public decimal TaxPercentage { get; set; }

    public List<TaxLineItem> Apply()
    {
        var lineItems = taxCalculator.Apply();
        lineItems.Add(new TaxLineItem { Description = $"Additional tax at {TaxPercentage}%", Tax = Price * TaxPercentage });
        return lineItems;
    }
}