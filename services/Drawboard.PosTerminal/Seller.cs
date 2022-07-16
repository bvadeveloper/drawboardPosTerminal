using Drawboard.PosTerminal.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Drawboard.PosTerminal;

public class Seller : IHostedService
{
    public Seller(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    private readonly IServiceProvider _serviceProvider;

    /// <inheritdoc/>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        using var saleScope = _serviceProvider.CreateScope();
        using var posTerminal = saleScope.ServiceProvider.GetRequiredService<IPosTerminal>();

        // ABCD
        // posTerminal.Scan("A");
        // posTerminal.Scan("B");
        // posTerminal.Scan("C");
        // posTerminal.Scan("D");

        // CCCCCCC
        // posTerminal.Scan("C");
        // posTerminal.Scan("C");
        // posTerminal.Scan("C");
        // posTerminal.Scan("C");
        // posTerminal.Scan("C");
        // posTerminal.Scan("C");
        // posTerminal.Scan("C");

        // ABCDABA
        posTerminal.Scan("A");
        posTerminal.Scan("B");
        posTerminal.Scan("C");
        posTerminal.Scan("D");
        posTerminal.Scan("A");
        posTerminal.Scan("B");
        posTerminal.Scan("A");

        // posTerminal.Scan("X");

        posTerminal.PrintReceipt();

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}