using System.ComponentModel.DataAnnotations;

namespace ConcertManagementSystem.Api.Models.Requests;

public class CreateTicketTypeRequest
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range(0.01, 100000, ErrorMessage = "Price must be greater than 0.01 and less than 100000")]
    public decimal Price { get; set; }

    [Required]
    [Range(1, 100000, ErrorMessage = "Total quantity must be between 1 and 100000")]
    public int TotalQuantity { get; set; }
}
