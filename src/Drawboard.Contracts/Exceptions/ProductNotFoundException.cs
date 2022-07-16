namespace Drawboard.Contracts.Exceptions;

/// <summary>
/// Product not found exception
/// </summary>
public class ProductNotFoundException : BusinessException
{
    public ProductNotFoundException(string message) : base(message)
    {
    }
}