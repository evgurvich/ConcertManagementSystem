using ConcertManagementSystem.Api.Models;

namespace ConcertManagementSystem.Api.Repositories;

public interface ITicketRepository
{
    void Add(Ticket ticket);
    Ticket? GetById(Guid ticketId);
    IEnumerable<Ticket> GetByEventId(Guid eventId);
    void Update(Ticket ticket);
}
