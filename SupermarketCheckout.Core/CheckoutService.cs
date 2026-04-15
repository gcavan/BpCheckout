namespace SupermarketCheckout.Core;

public sealed class CheckoutService : ICheckoutService
{
    private readonly IReadOnlyDictionary<string, IPriceRule> _rulesBySku;
    private readonly List<string> _scannedSkus = [];

    public CheckoutService(IEnumerable<IPriceRule> rules)
    {
        ArgumentNullException.ThrowIfNull(rules);

        _rulesBySku = rules.ToDictionary(rule => rule.Sku, rule => rule);
    }

    public bool Scan(string sku)
    {
        if (string.IsNullOrWhiteSpace(sku) || sku.Length != 1)
        {
            return false;
        }

        if (!_rulesBySku.ContainsKey(sku))
        {
            return false;
        }

        _scannedSkus.Add(sku);
        return true;
    }

    public int CalculateTotal()
    {
        if (_scannedSkus.Count == 0)
        {
            return 0;
        }

        var counts = _scannedSkus
            .GroupBy(sku => sku)
            .ToDictionary(group => group.Key, group => group.Count(), StringComparer.Ordinal);

        return counts.Sum(item => _rulesBySku[item.Key].CalculatePrice(item.Value));
    }

    public void Clear()
    {
        _scannedSkus.Clear();
    }
}
