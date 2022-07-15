using Drawboard.PosTerminal.Abstractions;

namespace Drawboard.PosTerminal;

/// <summary>
/// Console user notification implementation
/// </summary>
public class ConsoleMessenger : IMessenger
{
    /// <summary>
    /// Print warning message for user
    /// </summary>
    /// <param name="message"></param>
    public void ShowWarning(string message)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(message);
        Console.ResetColor();
        Console.WriteLine();
    }

    /// <summary>
    /// Print info message for user
    /// </summary>
    /// <param name="message"></param>
    public void ShowInfo(string message)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(message);
        Console.ResetColor();
        Console.WriteLine();
    }
}