using ConcertManagementSystem.Api.Models;

namespace ConcertManagementSystem.Api.Repositories;

public interface IEventRepository
{
    IEnumerable<Event> GetAll();
    Event? GetById(Guid id);
    void Add(Event concertEvent);
    void Update(Event concertEvent);
    void Delete(Guid id);
}
