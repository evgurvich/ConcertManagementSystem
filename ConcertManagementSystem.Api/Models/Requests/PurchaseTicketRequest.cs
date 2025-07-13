using System.ComponentModel.DataAnnotations;

namespace ConcertManagementSystem.Api.Models.Requests;

public class PurchaseTicketRequest
{
    [Required]
    public Guid TicketId { get; set; }
}