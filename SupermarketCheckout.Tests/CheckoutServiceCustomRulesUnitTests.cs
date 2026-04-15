using SupermarketCheckout.Core;

namespace SupermarketCheckout.Tests;

public class CheckoutServiceCustomRulesUnitTests
{
    private readonly ICheckoutService _checkoutService;

    public CheckoutServiceCustomRulesUnitTests()
    {
        _checkoutService = SetupCheckoutService();
    }

    private static ICheckoutService SetupCheckoutService()
    {
        var rules = new List<IPriceRule>
        {
            new SkuPriceRule("P", 3, offerQuantity: 3, offerPrice: 8),
            new SkuPriceRule("Q", 2, offerQuantity: 2, offerPrice: 3),
            new SkuPriceRule("R", 1),
        };

        return new CheckoutService(rules);
    }

    [Theory]
    [InlineData("", false)]
    [InlineData(" ", false)]
    [InlineData("PP", false)]
    [InlineData("A", false)]
    [InlineData("q", false)]
    [InlineData("1", false)]
    [InlineData("P", true)]
    [InlineData("Q", true)]
    [InlineData("R", true)]
    public void Scan_ReturnsExpectedResult(string sku, bool expectedResult)
    {
        var actual = _checkoutService.Scan(sku);
        Assert.Equal(expectedResult, actual);
    }

    [Fact]
    public void CalculateTotal_NoScannedItems_ReturnsZero()
    {
        Assert.Equal(0, _checkoutService.CalculateTotal());
    }

    [Theory]
    [InlineData("P", 3)]
    [InlineData("Q", 2)]
    [InlineData("R", 1)]
    [InlineData("PP", 6)]
    [InlineData("PPP", 8)]
    [InlineData("PPPP", 11)]
    public void CalculateTotal_ScannedItems_ReturnsExpectedTotal(string basket, int expectedTotal)
    {
        foreach (var skuChar in basket)
        {
            Assert.True(_checkoutService.Scan(skuChar.ToString()));
        }

        Assert.Equal(expectedTotal, _checkoutService.CalculateTotal());
    }

    [Fact]
    public void Clear_RemovesAllScannedItems()
    {
        Assert.True(_checkoutService.Scan("P"));
        Assert.True(_checkoutService.Scan("Q"));
        Assert.Equal(5, _checkoutService.CalculateTotal());

        _checkoutService.Clear();

        Assert.Equal(0, _checkoutService.CalculateTotal());
    }
}
