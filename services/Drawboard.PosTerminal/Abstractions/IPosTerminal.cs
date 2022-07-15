namespace Drawboard.PosTerminal.Abstractions;

public interface IPosTerminal : IDisposable
{
    /// <summary>
    /// Scan product code
    /// </summary>
    /// <param name="code">product code.</param>
    void Scan(string code);
    
    /// <summary>
    /// Print all receipt data
    /// </summary>
    void PrintReceipt();
}