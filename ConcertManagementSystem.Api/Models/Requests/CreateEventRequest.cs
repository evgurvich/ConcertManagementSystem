using System.ComponentModel.DataAnnotations;

namespace ConcertManagementSystem.Api.Models.Requests;

public class CreateEventRequest
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string Venue { get; set; } = string.Empty;

    [Required]
    public DateTime Date { get; set; }

    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(1, 100000, ErrorMessage = "Total capacity must be between 1 and 100000")]
    public int TotalCapacity { get; set; }
}