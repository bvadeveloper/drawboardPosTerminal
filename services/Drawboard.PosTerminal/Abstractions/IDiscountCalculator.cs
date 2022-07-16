using Drawboard.Entities.Entities;

namespace Drawboard.PosTerminal.Abstractions;

public interface IDiscountCalculator
{
    /// <summary>
    /// Calculate discount
    /// </summary>
    /// <param name="receiptItems"></param>
    /// <param name="product"></param>
    /// <returns></returns>
    public (string, decimal, int) Calculate(Dictionary<string, ReceiptItemEntity> receiptItems, ProductEntity product);
}