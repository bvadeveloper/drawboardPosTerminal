namespace Drawboard.PosTerminal.Abstractions;

/// <summary>
/// Base interface for user notification
/// </summary>
public interface IMessenger
{
    /// <summary>
    /// Show warning message for user
    /// </summary>
    /// <param name="message"></param>
    public void ShowWarning(string message);
    
    /// <summary>
    /// Show info message for user
    /// </summary>
    /// <param name="message"></param>
    public void ShowInfo(string message);
}