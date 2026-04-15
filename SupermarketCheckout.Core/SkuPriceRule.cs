namespace SupermarketCheckout.Core;

public sealed class SkuPriceRule : IPriceRule
{
    private readonly int _unitPrice;
    private readonly int? _offerQuantity;
    private readonly int? _offerPrice;

    public SkuPriceRule(string sku, int unitPrice, int? offerQuantity = null, int? offerPrice = null)
    {
        if (string.IsNullOrWhiteSpace(sku) || sku.Length != 1 || !char.IsUpper(sku[0]))
        {
            throw new ArgumentException("SKU must be a single uppercase letter.", nameof(sku));
        }

        if (unitPrice <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(unitPrice), "Unit price must be greater than zero.");
        }

        if (offerQuantity is not null && offerQuantity <= 1)
        {
            throw new ArgumentOutOfRangeException(nameof(offerQuantity), "Offer quantity must be greater than one.");
        }

        if (offerPrice is not null && offerPrice <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(offerPrice), "Offer price must be greater than zero.");
        }

        if ((offerQuantity is null) != (offerPrice is null))
        {
            throw new ArgumentException("Offer quantity and offer price must both be provided together.");
        }

        Sku = sku;
        _unitPrice = unitPrice;
        _offerQuantity = offerQuantity;
        _offerPrice = offerPrice;
    }

    public string Sku { get; }

    public int CalculatePrice(int quantity)
    {
        if (quantity < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity cannot be negative.");
        }

        if (quantity == 0)
        {
            return 0;
        }

        if (_offerQuantity is null || _offerPrice is null)
        {
            return quantity * _unitPrice;
        }

        var offerCount = quantity / _offerQuantity.Value;
        var remainder = quantity % _offerQuantity.Value;

        return (offerCount * _offerPrice.Value) + (remainder * _unitPrice);
    }
}
