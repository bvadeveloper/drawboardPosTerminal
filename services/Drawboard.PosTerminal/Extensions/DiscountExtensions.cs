using Drawboard.Entities.Entities;

namespace Drawboard.PosTerminal.Extensions;

public static class DiscountExtensions
{
    public static (string, decimal, int) CalculateDiscount(this ReceiptEntity receiptEntity, ProductEntity product)
    {
        // If the product is not found in the receipt items we add products with a unit price

        if (receiptEntity.ReceiptItems.TryGetValue(product.ProductCode, out ReceiptItemEntity receiptItem))
        {
            // If the product is found in the receipt items, but not enough for discount

            var sumCount = ++receiptItem.Count;

            if (sumCount < product.DiscountNumberItems)
            {
                var costWithoutDiscount = sumCount * product.UnitPrice;

                return (product.ProductCode, costWithoutDiscount, sumCount);
            }

            // If the product is found in the receipt items and enough for discount

            var itemCountWithoutDiscount = sumCount - product.DiscountNumberItems;

            // If the count of purchased matches the discount, return price with discount

            if (itemCountWithoutDiscount == 0)
            {
                return (product.ProductCode, product.PackPrice, sumCount);
            }

            // If the count of products more than items with discount, calculate total sum for product

            if (itemCountWithoutDiscount > 0)
            {
                var costWithDiscount = product.PackPrice;
                var costWithoutDiscount = itemCountWithoutDiscount * product.UnitPrice;
                var totalCost = costWithDiscount + costWithoutDiscount;

                return (product.ProductCode, totalCost, sumCount);
            }
        }

        return (product.ProductCode, product.UnitPrice, 1);
    }

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