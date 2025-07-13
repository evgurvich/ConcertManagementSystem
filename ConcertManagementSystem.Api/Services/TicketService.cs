using System.Threading.Tasks;
using ConcertManagementSystem.Api.Models;
using ConcertManagementSystem.Api.Models.Requests;
using ConcertManagementSystem.Api.Models.Responses;
using ConcertManagementSystem.Api.Repositories;

namespace ConcertManagementSystem.Api.Services;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IPaymentProcessingService _paymentProcessingService;

    public TicketService(ITicketRepository ticketRepository, IEventRepository eventRepository, IPaymentProcessingService paymentProcessingService)
    {
        _ticketRepository = ticketRepository;
        _eventRepository = eventRepository;
        _paymentProcessingService = paymentProcessingService;
    }

    public ReserveTicketResponse ReserveTicket(ReserveTicketRequest request)
    {
        var concertEvent = _eventRepository.GetById(request.EventId)
            ?? throw new ArgumentException("Event not found");

        var ticketType = concertEvent.TicketTypes.FirstOrDefault(tt => tt.Id == request.TicketTypeId)
            ?? throw new ArgumentException("Ticket type not found");

        var totalHeld = _ticketRepository
            .GetByEventId(request.EventId)
            .Count(t => t.TicketTypeId == request.TicketTypeId &&
                        (t.Status == TicketStatus.Reserved || t.Status == TicketStatus.Purchased));

        if (totalHeld >= ticketType.TotalQuantity)
            throw new InvalidOperationException("No tickets available for this type.");


        var ticket = new Ticket
        {
            EventId = request.EventId,
            TicketTypeId = request.TicketTypeId,
            Status = TicketStatus.Reserved,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddMinutes(15)
        };

        _ticketRepository.Add(ticket);

        return new ReserveTicketResponse
        {
            TicketId = ticket.Id,
            ExpiresAt = ticket.ExpiresAt
        };
    }


    public async Task<PurchaseTicketResponse> PurchaseTicket(PurchaseTicketRequest request)
    {
        var ticket = _ticketRepository.GetById(request.TicketId);
        if (ticket == null)
            throw new ArgumentException("Ticket not found");

        if (ticket.Status != TicketStatus.Reserved)
            throw new InvalidOperationException("Ticket is not reserved");

        if (ticket.ExpiresAt < DateTime.UtcNow)
            throw new InvalidOperationException("Ticket reservation expired");

        var concertEvent = _eventRepository.GetById(ticket.EventId)
            ?? throw new ArgumentException("Event not found");

        var ticketType = concertEvent.TicketTypes.FirstOrDefault(tt => tt.Id == ticket.TicketTypeId)
            ?? throw new ArgumentException("Ticket type not found");

        var success = await _paymentProcessingService.ProcessPaymentAsync(ticket.Id, ticketType.Price);

        if (!success)
            throw new InvalidOperationException("Payment failed");

        ticket.Status = TicketStatus.Purchased;
        ticket.PurchasedAt = DateTime.UtcNow;
        _ticketRepository.Update(ticket);

        return new PurchaseTicketResponse
        {
            TicketId = ticket.Id,
            PurchasedAt = ticket.PurchasedAt.Value
        };
    }

    public void CancelReservation(Guid ticketId)
    {
        var ticket = _ticketRepository.GetById(ticketId);
        if (ticket == null)
            throw new ArgumentException("Ticket not found");

        if (ticket.Status != TicketStatus.Reserved)
            throw new InvalidOperationException("Only reserved tickets can be cancelled");

        ticket.Status = TicketStatus.Cancelled;
        ticket.ExpiresAt = null;

        _ticketRepository.Update(ticket);
    }

}
