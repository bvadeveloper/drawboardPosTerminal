using Drawboard.Contracts.Enums;

namespace Drawboard.PosTerminal.Abstractions;

public interface IDiscountCalculatorFactory
{
    /// <summary>
    /// Make discount calculator by discount type
    /// </summary>
    /// <param name="discountType"></param>
    /// <returns></returns>
    public IDiscountCalculator Make(DiscountType discountType);
}