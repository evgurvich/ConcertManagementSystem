namespace ConcertManagementSystem.Api.Models.Responses;

public class ReserveTicketResponse
{
    public Guid TicketId { get; set; }

    public DateTime? ExpiresAt { get; set; }
}
