using ConcertManagementSystem.Api.Models;

namespace ConcertManagementSystem.Api.Repositories;

public class InMemoryTicketRepository : ITicketRepository
{
    private readonly List<Ticket> _tickets = new();

    public void Add(Ticket ticket)
    {
        _tickets.Add(ticket);
    }

    public Ticket? GetById(Guid ticketId)
    {
        return _tickets.FirstOrDefault(t => t.Id == ticketId);
    }

    public IEnumerable<Ticket> GetByEventId(Guid eventId)
    {
    
        return _tickets.Where(t => t.EventId == eventId);
    }

    public void Update(Ticket ticket)
    {
    }
}
