namespace SupermarketCheckout.Core;

public static class CheckoutServiceFactory
{
    public static ICheckoutService CreateDefault()
    {
        var defaultRules = new List<IPriceRule>
        {
            new SkuPriceRule("A", 50, offerQuantity: 3, offerPrice: 130),
            new SkuPriceRule("B", 30, offerQuantity: 2, offerPrice: 45),
            new SkuPriceRule("C", 20),
            new SkuPriceRule("D", 15),
        };

        return new CheckoutService(defaultRules);
    }
}
