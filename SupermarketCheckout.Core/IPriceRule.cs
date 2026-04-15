namespace SupermarketCheckout.Core;

public interface IPriceRule
{
    string Sku { get; }

    int CalculatePrice(int quantity);
}
