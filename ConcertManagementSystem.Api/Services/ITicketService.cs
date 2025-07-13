using ConcertManagementSystem.Api.Models;
using ConcertManagementSystem.Api.Models.Requests;
using ConcertManagementSystem.Api.Models.Responses;

namespace ConcertManagementSystem.Api.Services;

public interface ITicketService
{
    public ReserveTicketResponse ReserveTicket(ReserveTicketRequest request);
    public Task<PurchaseTicketResponse> PurchaseTicket(PurchaseTicketRequest request);
    void CancelReservation(Guid ticketId);
}
