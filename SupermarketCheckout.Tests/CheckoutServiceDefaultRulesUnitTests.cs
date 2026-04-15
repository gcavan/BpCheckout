using SupermarketCheckout.Core;

namespace SupermarketCheckout.Tests;

public class CheckoutServiceDefaultRulesUnitTests
{
    private readonly ICheckoutService _checkoutService;

    public CheckoutServiceDefaultRulesUnitTests()
    {
        _checkoutService = SetupCheckoutService();
    }

    private static ICheckoutService SetupCheckoutService()
    {
        return CheckoutServiceFactory.CreateDefault();
    }

    [Theory]
    [InlineData("", false)]
    [InlineData(" ", false)]
    [InlineData("AA", false)]
    [InlineData("E", false)]
    [InlineData("a", false)]
    [InlineData("1", false)]
    [InlineData("A", true)]
    [InlineData("B", true)]
    [InlineData("C", true)]
    [InlineData("D", true)]
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
    [InlineData("A", 50)]
    [InlineData("B", 30)]
    [InlineData("C", 20)]
    [InlineData("D", 15)]
    [InlineData("AA", 100)]
    [InlineData("AAA", 130)]
    [InlineData("AAAA", 180)]
    [InlineData("AAAAAA", 260)]
    [InlineData("BB", 45)]
    [InlineData("BBB", 75)]
    [InlineData("BBBB", 90)]
    [InlineData("ABCD", 115)]
    [InlineData("AAABB", 175)]
    [InlineData("AAABBD", 190)]
    [InlineData("AAAAABB", 275)]
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
        Assert.True(_checkoutService.Scan("A"));
        Assert.True(_checkoutService.Scan("B"));
        Assert.Equal(80, _checkoutService.CalculateTotal());

        _checkoutService.Clear();

        Assert.Equal(0, _checkoutService.CalculateTotal());
    }
}
