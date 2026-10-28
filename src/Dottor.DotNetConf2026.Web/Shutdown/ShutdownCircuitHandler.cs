using Microsoft.AspNetCore.Components.Server.Circuits;

namespace Dottor.DotNetConf2026.Web.Shutdown;

public sealed class ShutdownCircuitHandler(CircuitShutdownService shutdown,
    ILogger<ShutdownCircuitHandler> logger) : CircuitHandler
{
    public string? CircuitId { get; private set; }
    public bool IsConnected { get; private set; }

    public override Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        CircuitId = circuit.Id;
        logger.LogInformation("Apertura circuito: {CircuitId}", circuit.Id);
        return Task.CompletedTask;
    }

    public override Task OnConnectionUpAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        IsConnected = true;
        shutdown.Register(circuit);
        return Task.CompletedTask;
    }

    public override Task OnConnectionDownAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        IsConnected = false;
        shutdown.Unregister(circuit);
        return Task.CompletedTask;
    }

    public override Task OnCircuitClosedAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        shutdown.Unregister(circuit);
        logger.LogInformation("Chiusura circuito: {CircuitId}", circuit.Id);
        return Task.CompletedTask;
    }
}