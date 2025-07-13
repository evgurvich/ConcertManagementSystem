
namespace ConcertManagementSystem.Api.Models.Responses;

public class GetEventResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Venue { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public int TotalCapacity { get; set; }
}