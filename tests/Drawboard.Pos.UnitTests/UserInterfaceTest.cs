using Drawboard.PosTerminal.Abstractions;

namespace Drawboard.Pos.UnitTests;

public class UserInterfaceTest : IUserInterface
{
    internal string WarningMessage { get; set; }
    internal string InfoMessage { get; set; }

    public void ShowWarning(string message) => WarningMessage = message;

    public void ShowInfo(string message) => InfoMessage = message;
}