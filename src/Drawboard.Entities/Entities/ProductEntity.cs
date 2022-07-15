namespace Drawboard.Entities.Entities;

public record ProductEntity
{
    /// <summary>
    /// Empty constructor for initialization via IOptions
    /// </summary>
    public ProductEntity()
    {
    }

    public ProductEntity(string productCode, decimal unitPrice, decimal packPrice, int discountNumberItems)
    {
        ProductCode = productCode;
        UnitPrice = unitPrice;
        PackPrice = packPrice;
        DiscountNumberItems = discountNumberItems;
    }

    /// <summary>
    /// Unique product code in the product storage
    /// </summary>
    public string ProductCode { get; init; } = string.Empty;

    /// <summary>
    /// Product price per unit
    /// </summary>
    public decimal UnitPrice { get; init; }

    /// <summary>
    /// Product price per packet
    /// </summary>
    public decimal PackPrice { get; init; }

    /// <summary>
    /// Number of items for discount
    /// </summary>
    public int DiscountNumberItems { get; init; }
}