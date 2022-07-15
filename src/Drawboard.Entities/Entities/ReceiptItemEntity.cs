namespace Drawboard.Entities.Entities;

public record ReceiptItemEntity(string ProductCode, decimal Cost, int Count)
{
    public string ProductCode { get; set; } = ProductCode;

    public int Count { get; set; } = Count;

    public decimal Cost { get; set; } = Cost;
}