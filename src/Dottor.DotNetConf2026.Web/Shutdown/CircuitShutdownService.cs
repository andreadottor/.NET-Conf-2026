using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.Extensions.Options;

namespace Dottor.DotNetConf2026.Web.Shutdown;

public sealed class CircuitShutdownService : IDisposable
{
    private readonly object gate = new();
    private readonly Dictionary<string, Circuit> circuits = [];
    private readonly TaskCompletionSource drained = new(TaskCreationOptions.RunContinuationsAsynchronously);
    private readonly ILogger<CircuitShutdownService> logger;
    private readonly TimeSpan timeout;
    private readonly CancellationTokenRegistration stoppingRegistration;
    private bool stopping;
    private CancellationToken shutdownToken;

    public CircuitShutdownService(IHostApplicationLifetime lifetime,
        IOptions<ShutdownCircuitOptions> options, ILogger<CircuitShutdownService> logger)
    {
        this.logger = logger;
        timeout = options.Value.ShutdownTimeout;
        stoppingRegistration = lifetime.ApplicationStopping.Register(Drain);
    }

    public bool IsStopping { get { lock (gate) { return stopping; } } }

    public void Register(Circuit circuit)
    {
        bool requestPause;
        lock (gate)
        {
            circuits[circuit.Id] = circuit;
            requestPause = stopping;
        }
        logger.LogInformation("Circuito connesso: {CircuitId}", circuit.Id);
        if (requestPause)
        {
            _ = RequestPauseAsync(circuit, shutdownToken);
        }
    }

    public void Unregister(Circuit circuit)
    {
        lock (gate)
        {
            circuits.Remove(circuit.Id);
            if (stopping && circuits.Count == 0)
            {
                drained.TrySetResult();
            }
        }
        logger.LogInformation("Circuito disconnesso: {CircuitId}", circuit.Id);
    }

    private void Drain()
    {
        using var deadline = new CancellationTokenSource(timeout);
        Circuit[] snapshot;
        lock (gate)
        {
            stopping = true;
            shutdownToken = deadline.Token;
            snapshot = circuits.Values.ToArray();
            if (snapshot.Length == 0)
            {
                drained.TrySetResult();
            }
        }
        logger.LogInformation("Avvio shutdown: {Count} circuiti, timeout {Timeout}", snapshot.Length, timeout);
        try
        {
            // Acceptance is not persistence: wait for connection-down notifications as well.
            Task.WhenAll(snapshot.Select(c => RequestPauseAsync(c, deadline.Token)).Append(drained.Task))
                .WaitAsync(deadline.Token).GetAwaiter().GetResult();
            logger.LogInformation("Shutdown: tutti i circuiti connessi hanno completato la disconnessione");
        }
        catch (OperationCanceledException) when (deadline.IsCancellationRequested)
        {
            lock (gate)
            {
                logger.LogWarning("Timeout shutdown: {Count} circuiti ancora connessi; ripristino non garantito", circuits.Count);
            }
        }
        logger.LogInformation("Completamento drain shutdown; il processo host può terminare");
    }

    private async Task RequestPauseAsync(Circuit circuit, CancellationToken token)
    {
        try
        {
            logger.LogInformation("Richiesta sospensione: {CircuitId}", circuit.Id);
            var accepted = await circuit.RequestCircuitPauseAsync(token);
            logger.LogInformation("Richiesta sospensione {CircuitId}: accettata={Accepted} (non conferma il salvataggio)", circuit.Id, accepted);
        }
        catch (OperationCanceledException) when (token.IsCancellationRequested)
        {
            logger.LogWarning("Richiesta sospensione scaduta: {CircuitId}", circuit.Id);
        }
        catch (Exception exception)
        {
            // One failed client must not prevent the other circuits from draining.
            logger.LogError(exception, "Errore richiesta sospensione: {CircuitId}", circuit.Id);
        }
    }

    public void Dispose() => stoppingRegistration.Dispose();
}