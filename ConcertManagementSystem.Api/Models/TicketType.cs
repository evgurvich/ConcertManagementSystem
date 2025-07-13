namespace ConcertManagementSystem.Api.Models;

public class TicketType
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int TotalQuantity { get; set; }

    public Guid EventId { get; set; }
}
