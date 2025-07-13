namespace ConcertManagementSystem.Api.Models;

public enum TicketStatus
{
    Reserved,
    Purchased,
    Cancelled
}

public class Ticket
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid EventId { get; set; }
    public Guid TicketTypeId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? PurchasedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }

    public TicketStatus Status { get; set; } = TicketStatus.Reserved;
}
