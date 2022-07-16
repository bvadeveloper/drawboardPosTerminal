namespace Drawboard.PosTerminal.Abstractions;

/// <summary>
/// Interface for communication with user
/// </summary>
public interface IUserInterface
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