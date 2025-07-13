namespace ConcertManagementSystem.Api.Models;

public class Event
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Venue { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public int TotalCapacity { get; set; }

    public List<TicketType> TicketTypes { get; set; } = new List<TicketType>();
}