namespace ShoppingMart.Common.Utilities;


public static class ExtensionMethods
{
    public static bool IsStateWithHighTax(this string state)
    {
        state = state.ToUpperInvariant();
        return state switch
        {
            "CA" or "FL" or "IL" => true,
            _ => false,
        };
    }

    public static bool IsStateWithDiscountRules(this string state)
    {
        state = state.ToUpperInvariant();
        return state switch
        {
            "FL" or "NM" or "NV" => true,
            _ => false,
        };
    }
}