using Drawboard.Entities.Entities;
using Drawboard.PosTerminal.Abstractions;

namespace Drawboard.PosTerminal.Discounts;

/// <summary>
/// Discount calculator for products with a certain quantity
/// </summary>
public class CountOfProductsDiscountCalculator : IDiscountCalculator
{
    public (string, decimal, int) Calculate(Dictionary<string, ReceiptItemEntity> receiptItems, ProductEntity product)
    {
        // if the product is not found in the receipt items let's add products with a unit price

#pragma warning disable CS8600
        if (receiptItems.TryGetValue(product.ProductCode, out var item))
#pragma warning restore CS8600
        {
            // if the product is found in receipt items, but not enough for discount let's use unit price

            var sumCount = ++item.Count;

            if (sumCount < product.DiscountNumberItems)
            {
                var costWithoutDiscount = sumCount * product.UnitPrice;

                return (product.ProductCode, costWithoutDiscount, sumCount);
            }

            // calculate count of products without discount

            var itemCountWithoutDiscount = sumCount - product.DiscountNumberItems;

            // if the count of purchased matches the discount, return price with discount

            if (itemCountWithoutDiscount == 0)
            {
                return (product.ProductCode, product.PackPrice, sumCount);
            }

            // if the count of products more than items with discount, let's calculate total sum for product

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
}