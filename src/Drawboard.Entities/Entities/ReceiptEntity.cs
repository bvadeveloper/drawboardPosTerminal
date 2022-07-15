namespace Drawboard.Entities.Entities;

/// <summary>
/// Receipt entity
/// </summary>
public record ReceiptEntity
{
    /// <summary>
    /// Unique guid of receipt in the system
    /// </summary>
    public Guid ReceiptId { get; } = Guid.NewGuid();

    /// <summary>
    /// Receipt items.
    /// Key - product code
    /// Value - receipt item entity
    /// </summary>
    public Dictionary<string, ReceiptItemEntity> ReceiptItems { get; set; } = new();

    /// <summary>
    /// Init new instance
    /// </summary>
    public static ReceiptEntity New => new();
}