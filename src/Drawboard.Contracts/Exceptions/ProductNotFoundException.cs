namespace Drawboard.Contracts.Exceptions;

public class ProductNotFoundException : BusinessException
{
    public ProductNotFoundException(string message) : base(message)
    {
    }
}