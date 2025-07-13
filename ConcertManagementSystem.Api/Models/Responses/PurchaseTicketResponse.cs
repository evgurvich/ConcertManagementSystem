namespace ConcertManagementSystem.Api.Models.Responses;

public class PurchaseTicketResponse
{
    public Guid TicketId { get; set; }
    public DateTime PurchasedAt { get; set; }
}