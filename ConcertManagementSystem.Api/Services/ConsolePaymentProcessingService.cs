

namespace ConcertManagementSystem.Api.Services;

public class ConsolePaymentProcessingService : IPaymentProcessingService
{
    public Task<bool> ProcessPaymentAsync(Guid ticketId, decimal amount)
    {
        Console.WriteLine($"Processing payment for Ticket ID: {ticketId}, Amount: {amount}");
        return Task.FromResult(true);
    }
}