using Drawboard.PosTerminal.Abstractions;

namespace Drawboard.Pos.UnitTests;

public class TestMessenger : IMessenger
{
    internal string WarningMessage { get; set; }
    internal string InfoMessage { get; set; }

    public void ShowWarning(string message)
    {
        WarningMessage = message;
    }

    public void ShowInfo(string message)
    {
        InfoMessage = message;
    }
}