using System.Net;
using Dottor.DotNetConf2026.Web.Models;
using Dottor.DotNetConf2026.Web.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Dottor.DotNetConf2026.Web.Endpoints;

public static class DemoEndpoints
{
    public static void MapDemoEndpoints(this WebApplication app)
    {
        app.MapGet("/health", (DemoInstance instance) => Results.Ok(new
        {
            status = "Healthy", instance.Version, instance.ProcessId
        }));

        app.MapPost("/api/users", (UserRegistration model, UserDirectory users) =>
        {
            var user = users.TryCreate(model);
            return user is not null
                ? Results.Created($"/api/users/{user.Id}", user)
                : Results.Conflict(new { message = "L'email è stata registrata da un'altra richiesta. Riprovare." });
        }).WithSummary("Registrazione con DataAnnotations sincrone e asincrone native");

        app.MapGet("/api/tickets/{id:int}/lookup", (int id, TicketCatalog tickets) => Lookup(id, tickets))
            .WithSummary("Union C# 15: ticket oppure motivazione di indisponibilità (HTTP 200 in entrambi i casi)");

        app.MapGet("/api/tickets/{id:int}", (int id, TicketCatalog tickets) =>
            tickets.Find(id) is { } ticket
                ? Results.Ok(ticket)
                : Results.Problem(statusCode: StatusCodes.Status404NotFound,
                    title: "Ticket non trovato",
                    detail: $"Il ticket {id} non è presente nell'archivio in memoria."))
            .Produces<Ticket>().ProducesProblem(StatusCodes.Status404NotFound);

#pragma warning disable CS0618 // The handler is intentionally obsolete: OpenAPI exports it as deprecated.
        app.MapGet("/api/tickets/{id:int}/legacy", LegacyTicketById)
            .WithSummary("Contratto legacy: [Obsolete] diventa deprecated: true nel documento OpenAPI");
#pragma warning restore CS0618

        if (app.Environment.IsDevelopment())
        {
            app.MapPost("/demo/shutdown", (HttpContext context, IHostApplicationLifetime lifetime) =>
            {
                var address = context.Connection.RemoteIpAddress;
                if (address is null || !IPAddress.IsLoopback(address) ||
                    context.Request.Headers["X-Demo-Deployment"] != "local")
                {
                    return Results.Problem("Shutdown consentito solo da loopback con header X-Demo-Deployment: local.", statusCode: 403);
                }

                context.Response.OnCompleted(() =>
                {
                    ThreadPool.QueueUserWorkItem(_ => lifetime.StopApplication());
                    return Task.CompletedTask;
                });
                return Results.Accepted(value: new { message = "Shutdown controllato richiesto" });
            }).ExcludeFromDescription();
        }
    }

    private static TicketLookup Lookup(int id, TicketCatalog tickets) => tickets.Find(id) is { } ticket
        ? new TicketLookup(ticket)
        : new TicketLookup(new TicketUnavailable(id, "Ticket non presente nell'archivio in memoria."));

    [Obsolete("Usare GET /api/tickets/{id}.")]
    private static Results<Ok<LegacyTicket>, NotFound> LegacyTicketById(int id, TicketCatalog tickets) =>
        tickets.Find(id) is { } ticket
            ? TypedResults.Ok(new LegacyTicket(ticket.Id, ticket.Title, ticket.Priority)
            {
                Severity = ticket.Priority >= 3 ? "alta" : "normale"
            })
            : TypedResults.NotFound();
}