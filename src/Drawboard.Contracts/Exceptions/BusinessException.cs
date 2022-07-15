namespace Drawboard.Contracts.Exceptions;

/// <summary>
/// Base class for business exception in the system
/// </summary>
public abstract class BusinessException : Exception
{
    public Guid ErrorContextId { get; }

    protected BusinessException(string message) : base(message) =>
        ErrorContextId = Guid.NewGuid();
}