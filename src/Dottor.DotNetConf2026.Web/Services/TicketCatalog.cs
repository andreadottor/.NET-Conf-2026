using Dottor.DotNetConf2026.Web.Models;

namespace Dottor.DotNetConf2026.Web.Services;

public sealed class TicketCatalog
{
    private readonly Ticket[] tickets = Enumerable.Range(1, 240).Select(id => new Ticket(
        id, $"Ticket {id:000} - {new[] { "Accesso", "Report", "Notifiche", "Profilo" }[id % 4]}",
        id % 4 + 1, new[] { "Aperto", "In lavorazione", "Risolto" }[id % 3],
        new DateTime(2026, 10, 1).AddHours(id * 3), new[] { "Anna", "Marco", "Giulia" }[id % 3])).ToArray();

    public IQueryable<Ticket> All => tickets.AsQueryable();
    public Ticket? Find(int id) => tickets.FirstOrDefault(ticket => ticket.Id == id);
}