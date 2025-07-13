using ConcertManagementSystem.Api.Models;

namespace ConcertManagementSystem.Api.Repositories;

public class InMemoryEventRepository : IEventRepository
{
    private readonly List<Event> _events = new();

    public IEnumerable<Event> GetAll() => _events;

    public Event? GetById(Guid id) => _events.FirstOrDefault(e => e.Id == id);

    public void Add(Event concertEvent)
    {
        _events.Add(concertEvent);
    }

    public void Update(Event updatedEvent)
    {
        var index = _events.FindIndex(e => e.Id == updatedEvent.Id);
        if (index >= 0)
            _events[index] = updatedEvent;
    }

    public void Delete(Guid id)
    {
        var existing = _events.FirstOrDefault(e => e.Id == id);
        if (existing is not null)
            _events.Remove(existing);
    }
}
