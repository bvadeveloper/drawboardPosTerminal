namespace Drawboard.Contracts.Exceptions;

/// <summary>
/// Base class of exception for business logic
/// </summary>
public abstract class BusinessException : Exception
{
    public Guid ErrorContextId { get; }

    protected BusinessException(string message) : base(message) =>
        ErrorContextId = Guid.NewGuid();
}