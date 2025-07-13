using System.ComponentModel.DataAnnotations;

namespace ConcertManagementSystem.Api.Models.Requests;

public class GetEventRequest
{
    [Required]
    public Guid Id{ get; set; } 
}