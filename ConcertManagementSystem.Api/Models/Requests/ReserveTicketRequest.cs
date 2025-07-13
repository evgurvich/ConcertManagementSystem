using System.ComponentModel.DataAnnotations;

namespace ConcertManagementSystem.Api.Models.Requests;

public class ReserveTicketRequest
{
    [Required]
    public Guid EventId { get; set; }

    [Required]
    public Guid TicketTypeId { get; set; }
}
