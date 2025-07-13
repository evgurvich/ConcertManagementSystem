namespace ConcertManagementSystem.Api.Services;

public interface IPaymentProcessingService
{
    Task<bool> ProcessPaymentAsync(Guid ticketId, decimal amount);
}

