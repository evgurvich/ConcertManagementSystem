using System.Net;
using ConcertManagementSystem.Api.Models.Requests;
using ConcertManagementSystem.Api.Models.Responses;
using ConcertManagementSystem.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConcertManagementSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly ITicketService _ticketService;

    public TicketsController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    /// <summary>
    /// Reserves a ticket of a specific type for a concert event.
    /// </summary>
    /// <param name="request">Reservation request with event and ticket type IDs.</param>
    /// <returns>Information about the reserved ticket.</returns>
    /// <response code="200">Ticket reserved successfully.</response>
    /// <response code="400">Request format is invalid.</response>
    /// <response code="409">Ticket type is sold out.</response>
    /// <response code="500">Unexpected error occurred.</response>
    [HttpPost("reserve")]
    [ProducesResponseType(typeof(ReserveTicketResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public ActionResult<ReserveTicketResponse> ReserveTicket(ReserveTicketRequest request)
    {
        try
        {
            var result = _ticketService.ReserveTicket(request);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"ReserveTicket -- Error reserving ticket: {ex.Message}");
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"ReserveTicket -- Error reserving ticket: {ex.Message}");
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ReserveTicket -- Unexpected error: {ex}");
            return StatusCode((int)HttpStatusCode.InternalServerError, "An unexpected error occurred.");
        }
    }

    /// <summary>
    /// Processes the purchase of a reserved ticket.
    /// </summary>
    /// <param name="request">The purchase request containing the ticket ID.</param>
    /// <returns>Information about the purchased ticket, including timestamp.</returns>
    /// <response code="200">Ticket purchased successfully.</response>
    /// <response code="400">Ticket, event, or ticket type not found.</response>
    /// <response code="409">Ticket is not in a reserved state, expired, or payment failed.</response>
    /// <response code="500">Unexpected server error.</response>
    /// <remarks>
    /// Retrieves the ticket, event, and ticket type to determine the correct price,
    /// then calls the asynchronous payment processing service. If payment succeeds,
    /// the ticket is marked as purchased and updated.
    /// </remarks>
    [HttpPost("purchase")]
    [ProducesResponseType(typeof(PurchaseTicketResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<PurchaseTicketResponse>> Purchase([FromBody] PurchaseTicketRequest request)
    {
        try
        {
            var result = await _ticketService.PurchaseTicket(request);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"PurchaseTicket -- Error purchasing ticket: {ex.Message}");
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"PurchaseTicket -- Error purchasing ticket: {ex.Message}");
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"PurchaseTicket -- Unexpected error: {ex}");
            return StatusCode((int)HttpStatusCode.InternalServerError, "An unexpected error occurred.");
        }
    }


    /// <summary>
    /// Cancels a reserved ticket before it is purchased.
    /// </summary>
    /// <param name="ticketId">The ID of the ticket to cancel.</param>
    /// <response code="204">Reservation cancelled successfully.</response>
    /// <response code="404">Ticket not found.</response>
    /// <response code="409">Ticket is already purchased and cannot be cancelled.</response>
    /// <response code="500">Unexpected server error.</response>
    [HttpDelete("{ticketId}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public IActionResult CancelReservation(Guid ticketId)
    {
        try
        {
            _ticketService.CancelReservation(ticketId);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"CancelReservation -- Error cancelling the reservation: {ex.Message}");
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"CancelReservation -- Error cancelling the reservation: {ex.Message}");
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"CancelReservation -- Unexpected error: {ex.Message}");
            return StatusCode((int)HttpStatusCode.InternalServerError, "An unexpected error occurred.");
        }
    }


}