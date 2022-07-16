using Drawboard.PosTerminal.Abstractions;

namespace Drawboard.PosTerminal;

/// <summary>
/// Console user notification implementation
/// </summary>
public class UserInterfaceConsole : IUserInterface
{
    /// <summary>
    /// Print warning message for user
    /// </summary>
    /// <param name="message"></param>
    public void ShowError(string message) => WriteToConsole(message, ConsoleColor.Red);

    /// <summary>
    /// Print warning message for user
    /// </summary>
    /// <param name="message"></param>
    public void ShowWarning(string message) => WriteToConsole(message, ConsoleColor.Yellow);

    /// <summary>
    /// Print info message for user
    /// </summary>
    /// <param name="message"></param>
    public void ShowInfo(string message) => WriteToConsole(message, ConsoleColor.Green);

    private static void WriteToConsole(string message, ConsoleColor color)
    {
        Console.WriteLine();
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
        Console.WriteLine();
    }
}