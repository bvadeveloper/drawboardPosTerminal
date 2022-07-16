using Drawboard.Entities.Entities;

namespace Drawboard.PosTerminal.Extensions;

public static class ReceiptExtensions
{
    public static void AddOrUpdateItem(this Dictionary<string, ReceiptItemEntity> receiptItems, string productCode,
        decimal cost, int count)
    {
        if (receiptItems.ContainsKey(productCode))
        {
            receiptItems.Remove(productCode);
        }

        receiptItems.Add(productCode, new ReceiptItemEntity(productCode, cost, count));
    }
}