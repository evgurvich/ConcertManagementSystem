using ConcertManagementSystem.Api.Models;

namespace ConcertManagementSystem.Api.Models.Responses;

public class GetAllEventsResponse
{
    public List<Event> Events { get; set; } = new List<Event>();
}