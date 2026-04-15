namespace SupermarketCheckout.Core;

public interface ICheckoutService
{
    int CalculateTotal();

    bool Scan(string sku);

    void Clear();
}
