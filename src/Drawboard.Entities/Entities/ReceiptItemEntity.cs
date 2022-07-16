namespace Drawboard.Entities.Entities;

/// <summary>
/// Receipt item entity
/// </summary>
/// <param name="ProductCode"></param>
/// <param name="Cost"></param>
/// <param name="Count"></param>
public record ReceiptItemEntity(string ProductCode, decimal Cost, int Count)
{
    /// <summary>
    /// Product code
    /// </summary>
    public string ProductCode { get; set; } = ProductCode;

    /// <summary>
    /// Count of product
    /// </summary>
    public int Count { get; set; } = Count;

    /// <summary>
    /// Cost of product
    /// </summary>
    public decimal Cost { get; set; } = Cost;
}