using Drawboard.Contracts.Enums;
using Drawboard.PosTerminal.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Drawboard.PosTerminal.Discounts;

/// <summary>
/// Discount calculator factory
/// </summary>
public class DiscountCalculatorFactory : IDiscountCalculatorFactory
{
    private readonly IServiceProvider _serviceProvider;

    public DiscountCalculatorFactory(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    /// <summary>
    /// Make discount calculator by discount type
    /// </summary>
    /// <param name="discountType"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public IDiscountCalculator Make(DiscountType discountType)
    {
        switch (discountType)
        {
            case DiscountType.CountOfProducts:
                return _serviceProvider.GetRequiredService<CountOfProductsDiscountCalculator>();

            case DiscountType.Weekend:
            case DiscountType.BlackFriday:
            default:
                throw new NotImplementedException($"Discount type not implemented '{nameof(discountType)}'");
        }
    }
}