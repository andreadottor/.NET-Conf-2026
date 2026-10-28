namespace Dottor.DotNetConf2026.Web.Models;

public sealed record Ticket(int Id, string Title, int Priority, string Status, DateTime CreatedAt, string AssignedTo);

public sealed record TicketUnavailable(int RequestedId, string Reason);

public union TicketLookup(Ticket, TicketUnavailable);