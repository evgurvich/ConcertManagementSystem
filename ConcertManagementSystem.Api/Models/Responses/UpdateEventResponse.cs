namespace ConcertManagementSystem.Api.Models.Responses;

public class UpdateEventResponse
{
    public Guid Id { get; set; }
    public string Message { get; set; } = "Event updated successfully";
}